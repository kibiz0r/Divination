namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type Divinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (contextualize) =
    interface IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Contextualize (context) =
            contextualize context

module Divinable =
    //let cast (divinable : 'T when 'T :> IDivinableBase<'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : IDivinable<'U, 'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    //    match divinable :> obj with
    //    | :? IDivinable<'U, 'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> as casted ->
    //        casted
    //    | _ ->
    //        Divinable<'U, 'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (fun context ->
    //            divinable.Contextualize context
    //        ) :> _

    //let mergeScope (overridingScope : IdentificationScope<_, _, _, _, _, _, _>) (divinable : IDivinable<'T, _, _, _, _, _, _, _>) : IDivinable<'T, _, _, _, _, _, _, _> =
    //    Divinable<'T, _, _, _, _, _, _, _> (fun (originalScope, diviner) ->
    //        divinable.Identify (IdentificationScope.merge overridingScope originalScope, diviner)
    //    ) :> _

    let let' (var : IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)
        (argument : IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)
        (body : IDivinable<'U, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)
        : IDivinable<'U, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
        Divinable<'U, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (fun context ->
            context.Let (var, argument, body)
        ) :> _

    let var (name : string) : IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
        Divinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (fun context ->
            context.Identity (VarIdentity name)
        ) :> _

    let value (value : 'Value, type' : 'Type) : IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
        Divinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (fun context ->
            context.Identity (ValueIdentity (value, type'))
        ) :> _

//[<AutoOpen>]
//module DivinableExtensions =
//    type IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
//        member this.Divine (scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, diviner : IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
//            let identity = this.Identify (scope, diviner)
//            diviner.Resolve<'T> (scope, identity)