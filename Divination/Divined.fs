namespace Divination

open System
open System.Reflection
open FSharp.Reflection

//type IDivinedBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
//    interface
//    end

//type IDivined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
//    inherit IDivinedBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

// A Divined is a handle to a value that was materialized by giving an Identity to a Diviner.
type Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    | DivinedValue of Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> * 'T
    | DivinedException of Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> * exn
with
    member this.Identity =
        match this with
        | DivinedValue (i, _) -> i
        | DivinedException (i, _) -> i

    member this.Value =
        match this with
        | DivinedValue (_, v) -> v
        | DivinedException (_, e) -> raise e

    member this.Exception =
        match this with
        | DivinedValue (_, _) -> null
        | DivinedException (_, e) -> e

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Divined =
    let cast (divined : Divined<'T, _, _, _, _, _, _, _>) : Divined<'U, _, _, _, _, _, _, _> =
        match divined with
        | DivinedValue (identity, value) ->
            DivinedValue (identity, (value :> obj) :?> 'U)
        | DivinedException (identity, exception') ->
            DivinedException (identity, exception')