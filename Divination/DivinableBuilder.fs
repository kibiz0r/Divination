namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Evaluator

module Divinable =
    type DivineBuilder () =
        member this.Return (value : 'T) : 'T =
            value
            //DivinableExpr.Value (value, typeof<'T>) |> DivinableExpr.Cast

        member this.Quote (expr : Expr<'T>) =
            expr

        member this.Run (expr : Expr<'T>) : DivinableExpr<'T> =
            //let returnMethodInfo = typeof<DivineBuilder>.GetMethod "Return"
            //let innerExpr =
            //    match expr with
            //    | Patterns.Call (Some _, returnMethodInfo, [innerExpr]) -> innerExpr
            //    | _ -> raise (Exception (sprintf "Return method not found in expression: %A" expr))

            DivinableExpr.FromExpr expr |> DivinableExpr.Cast
    
    let divine = new DivineBuilder ()