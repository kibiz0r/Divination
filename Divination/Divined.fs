namespace Divination

open System
open System.Reflection

// A Divined is a handle to a value that was materialized by giving an Identity to a Diviner.
type Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    | DivinedValue of Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> * 'T
    | DivinedException of Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> * exn
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

type Divined<'T, 'Identifier, 'Value, 'Type> = Divined<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo>

type Divined<'T, 'Identifier, 'Value> = Divined<'T, 'Identifier, 'Value, Type>

type Divined<'T, 'Identifier> = Divined<'T, 'Identifier, obj>

type Divined<'T> = Divined<'T, obj>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Divined =
    let cast (divined : Divined<'T, _, _, _, _, _, _>) : Divined<'U, _, _, _, _, _, _> =
        match divined with
        | DivinedValue (identity, value) ->
            DivinedValue (identity, (value :> obj) :?> 'U)
        | DivinedException (identity, exception') ->
            DivinedException (identity, exception')