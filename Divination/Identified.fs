﻿namespace Divination

open System
open System.Reflection
open FSharp.Reflection

// An Identified is a handle to a value that has had its Identity tracked.
type Identified<'T, 'Identifier> =
    | IdentifiedValue of Identity<'Identifier> * 'T
    | IdentifiedException of Identity<'Identifier> * exn
with
    member this.Identity =
        match this with
        | IdentifiedValue (i, _) -> i
        | IdentifiedException (i, _) -> i

    member this.Value =
        match this with
        | IdentifiedValue (_, v) -> v
        | IdentifiedException (_, e) -> raise e

    member this.Exception =
        match this with
        | IdentifiedValue (_, _) -> null
        | IdentifiedException (_, e) -> e
