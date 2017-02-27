namespace Divination

open System

// Not entirely sure that having a non-generic form is a good idea, since return type info is used during dynamic dispatch...
//type Divined =
//    | DivinedValue of Identity * obj
//    | DivinedException of Identity * exn
//with
//    member this.Identity =
//        match this with
//        | DivinedValue (i, _) -> i
//        | DivinedException (i, _) -> i

//    member this.Value =
//        match this with
//        | DivinedValue (_, v) -> v
//        | DivinedException (_, e) -> raise e

//    member this.Exception =
//        match this with
//        | DivinedValue (_, _) -> null
//        | DivinedException (_, e) -> e

type Divined<'T> =
    | DivinedValue of Identity * 'T
    | DivinedException of Identity * exn
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