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
            | ValueWithName (_, type', name) ->
                VarIdentity (name, type')
            | Value (value, type') ->
                ValueIdentity (value, type')
            | Call (this', methodInfo, arguments) ->
                let this'' =
                    match this' with
                    | Some t -> Some (t.ToIdentity ())
                    | None -> None
                let arguments' = List.map (fun (argument : Expr) -> argument.ToIdentity ()) arguments
                CallIdentity (this'', methodInfo, arguments')
            | Coerce (argument, type') ->
                CoerceIdentity (argument.ToIdentity (), type')
            | NewUnionCase (unionCaseInfo, arguments) ->
                let arguments' = List.map (fun (argument : Expr) -> argument.ToIdentity ()) arguments
                NewUnionCaseIdentity (unionCaseInfo, arguments')
            | _ -> invalidOp (this.ToString ())