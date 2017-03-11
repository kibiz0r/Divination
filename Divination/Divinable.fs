namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type Divinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (identify : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> * IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> -> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) =
    interface IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Identify (scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, diviner : IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            identify (scope, diviner)

type Divinable<'T, 'Identifier, 'Value, 'Type> = Divinable<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type Divinable<'T, 'Identifier, 'Value> = Divinable<'T, 'Identifier, 'Value, Type>

type Divinable<'T, 'Identifier> = Divinable<'T, 'Identifier, obj>

type Divinable<'T> = Divinable<'T, obj>

module Divinable =
    let mergeScope (overridingScope : IdentificationScope<_, _, _, _, _, _, _>) (divinable : IDivinable<'T, _, _, _, _, _, _, _>) : IDivinable<'T, _, _, _, _, _, _, _> =
        Divinable<'T, _, _, _, _, _, _, _> (fun (originalScope, diviner) ->
            divinable.Identify (IdentificationScope.merge overridingScope originalScope, diviner)
        ) :> _

    let let' (var : IDivinable<'T, _>) (argument : IDivinable<'T, _>) (body : IDivinable<'U, _>) : IDivinable<'U, _> =
        Divinable<'U, _> (fun (scope, diviner) ->
            let var' = var.Identify (scope, diviner)
            let argument' = argument.Identify (scope, diviner)
            let body' = body.Identify (scope, diviner)
            LetIdentity (var', argument', body')
        ) :> _

    let var (name : string) : IDivinable<'T, _> =
        Divinable<'T, _, _, _, _, _, _, _> (fun (scope, diviner) ->
            VarIdentity (name, typeof<'T>)
        ) :> _

    let value (value : 'T) : IDivinable<'T, _> =
        Divinable<'T, _> (fun (scope, diviner) ->
            ValueIdentity (value :> obj, typeof<'T>)
        ) :> _

[<AutoOpen>]
module DivinableExtensions =
    type IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Divine (scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, diviner : IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            let identity = this.Identify (scope, diviner)
            diviner.Resolve<'T> (scope, identity)