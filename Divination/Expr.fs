namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns

[<AutoOpen>]
module Expr =
    type Expr with
        member this.ToIdentity () =
            match this with
            | Var (var) ->
                VarIdentity (var.Name, var.Type)
            | ValueWithName (_, type', name) ->
                VarIdentity (name, type')
            | Value (value, type') ->
                ValueIdentity (value, type')
            | NewObject (constructorInfo, arguments) ->
                let arguments' = List.map (fun (argument : Expr) -> argument.ToIdentity ()) arguments
                ConstructorIdentity (constructorInfo, arguments')
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
            | Let (var, argument, body) ->
                LetIdentity (VarIdentity (var.Name, var.Type), argument.ToIdentity (), body.ToIdentity ())
            | _ -> invalidOp (this.ToString ())