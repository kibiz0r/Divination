namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns

[<AutoOpen>]
module Expr =
    type Expr with
        member this.ToIdentity () =
            //ExprIdentifier.Current.ToIdentity this
            match this with
            | Var (var) ->
                VarIdentity (var.Name)
            | ValueWithName (_, _, name) ->
                VarIdentity (name)
            | Value (value, type') ->
                ValueIdentity (value, type')
            | NewObject (constructorInfo, arguments) ->
                let arguments' = List.map (fun (argument : Expr) -> argument.ToIdentity ()) arguments
                NewObjectIdentity (constructorInfo, arguments')
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
                LetIdentity (VarIdentity (var.Name), argument.ToIdentity (), body.ToIdentity ())
            | _ -> invalidOp (this.ToString ())