namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns

[<AutoOpen>]
module Expr =
    type Expr with
        member this.ToIdentity () =
            match this with
            | NewObject (constructorInfo, arguments) ->
                let arguments' = List.map (fun (argument : Expr) -> argument.ToIdentity ()) arguments
                ConstructorIdentity (constructorInfo, arguments')
            | Value (value, type') ->
                ValueIdentity (value, type')
            | _ -> invalidOp (this.ToString ())