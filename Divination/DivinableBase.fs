namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type DivinableBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (identify : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> * IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> -> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) =
    interface IDivinableBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Identify (scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, diviner : IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            identify (scope, diviner)

type DivinableBase<'Identifier, 'Value, 'Type> = DivinableBase<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type DivinableBase<'Identifier, 'Value> = DivinableBase<'Identifier, 'Value, Type>

type DivinableBase<'Identifier> = DivinableBase<'Identifier, obj>

type DivinableBase = DivinableBase<obj>

module DivinableBase =
    //let mergeScope (overridingScope : IdentificationScope<_, _, _, _, _, _, _>) (divinable : IDivinable<'T, _, _, _, _, _, _, _>) : IDivinable<'T, _, _, _, _, _, _, _> =
    //    Divinable<'T, _, _, _, _, _, _, _> (fun (originalScope, diviner) ->
    //        divinable.Identify (IdentificationScope.merge overridingScope originalScope, diviner)
    //    ) :> _

    let let' (var : IDivinableBase<_>) (argument : IDivinableBase<_>) (body : IDivinableBase<_>) : IDivinableBase<_> =
        DivinableBase<_> (fun (scope, diviner) ->
            let var' = var.Identify (scope, diviner)
            let argument' = argument.Identify (scope, diviner)
            let body' = body.Identify (scope, diviner)
            LetIdentity (var', argument', body')
        ) :> _

    let var (name : string) : IDivinableBase<_> =
        DivinableBase<_, _, _, _, _, _, _> (fun (scope, diviner) ->
            VarIdentity (name)
        ) :> _

    let value (value : obj) (type' : Type) : IDivinableBase<_> =
        DivinableBase<obj, _> (fun (scope, diviner) ->
            ValueIdentity (value, type')
        ) :> _

    let call (this : IDivinableBase<_> option) (methodInfo : MethodInfo) (arguments : IDivinableBase<_> list) : IDivinableBase<_> =
        DivinableBase<_> (fun (scope, diviner) ->
            let this' =
                match this with
                | Some t -> Some (t.Identify (scope, diviner))
                | None -> None
            let arguments' = List.map (fun (a : IDivinableBase<_>) -> a.Identify (scope, diviner)) arguments
            CallIdentity (this', methodInfo, arguments')
        ) :> _

    let newObject (constructorInfo : ConstructorInfo) (arguments : IDivinableBase<_> list) : IDivinableBase<_> =
        DivinableBase<_> (fun (scope, diviner) ->
            let arguments' = List.map (fun (a : IDivinableBase<_>) -> a.Identify (scope, diviner)) arguments
            NewObjectIdentity (constructorInfo, arguments')
        ) :> _