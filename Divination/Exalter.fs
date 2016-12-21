namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.ExprShape
open FSharp.Quotations.Evaluator

type Exalter () =
    interface IExalter with
        member this.Exalt<'T> (toExalt : Expr<'T>) : Divinable<'T> =
            let rec exalt expr =
                match expr with
                | Let (var, value, body) -> Divinable.let' (Divinable.var (var.Name, var.Type), exalt value, exalt body)
                | Value (value) -> Divinable.value value
                | Var (var) -> Divinable.varGet (var.Name)
                | _ -> raise (Exception "whoops")
            exalt toExalt |> Divinable.cast

        //member this.Exalt<'T> (toExalt : Expr<'T>) : Expr<IDivinable<'T>> =
        //    let rec exalt e =
        //        match e with
        //        | Let (var, expr, body) ->
        //            let var' = Var (var.Name, divinableType var.Type)
        //            let expr' = exalt expr
        //            let body' =
        //                match body with
        //                | Var (v) when v = var ->
        //                    Expr.Var var'
        //                | _ -> body
        //            Expr.Let (var', expr', body')
        //        | Value (v, t) ->
        //            let methodInfo = typeof<Divinable>.GetMethod "Value"
        //            let valueMethod = methodInfo.MakeGenericMethod t
        //            Expr.Call (valueMethod, [e])
        //        | _ -> <@@ Divinable.Value<'T> %%e @@>
        //    exalt toExalt |> Expr.Cast<IDivinable<'T>>