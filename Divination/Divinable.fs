namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type Divinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> (identify : IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> * IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> -> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) =
    interface IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Identify (diviner : IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            identify (diviner, scope)

type Divinable<'T, 'Identifier, 'Value, 'Type> = Divinable<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type Divinable<'T, 'Identifier, 'Value> = Divinable<'T, 'Identifier, 'Value, Type>

type Divinable<'T, 'Identifier> = Divinable<'T, 'Identifier, obj>

type Divinable<'T> = Divinable<'T, obj>

module Divinable =
    let mergeScope (overridingScope : IdentificationScope<_, _, _, _, _, _, _>) (divinable : IDivinable<'T, _, _, _, _, _, _, _>) : IDivinable<'T, _, _, _, _, _, _, _> =
        Divinable<'T, _, _, _, _, _, _, _> (fun (diviner, originalScope) -> divinable.Identify (diviner, IdentificationScope.merge overridingScope originalScope)) :> _

[<AutoOpen>]
module DivinableExtensions =
    type IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Divine (diviner : IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            let identity = this.Identify (diviner, scope)
            diviner.Resolve<'T> (identity, scope)