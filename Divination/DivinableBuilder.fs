namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<Divinable<'T>>, body : 'T -> Divinable<'U>) : Divinable<'U> =
        printfn "%A" divinableExpr
        Divinable<'U> <| fun () ->
            let divinable = divinableExpr |> QuotationEvaluator.Evaluate
            (body divinable.Value).Value

    member this.Return ([<ReflectedDefinition>] valueExpr : Expr<'T>) : Divinable<'T> =
        printfn "%A" valueExpr
        Divinable<'T> <| fun () ->
            valueExpr |> QuotationEvaluator.Evaluate