namespace Divination

open System

// A Divined is a handle to a value that was materialized by giving an Identity to a Diviner.
type Divined<'T, 'Identifier> =
    | DivinedValue of Identity<'Identifier> * 'T
    | DivinedException of Identity<'Identifier> * exn
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
    let cast (divined : Divined<'T, 'Identifier>) : Divined<'U, 'Identifier2> =
        match divined with
        | DivinedValue (identity, value) ->
            DivinedValue (Identity.cast identity, value :> obj :?> 'U)
        | DivinedException (identity, exception') ->
            DivinedException (Identity.cast identity, exception')