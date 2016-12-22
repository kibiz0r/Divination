namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Evaluator

type DivinedBuilder (exalter : IExalter, diviner : IDiviner) =
    member this.Bind (divinable : Divinable<'T>, f : 'T -> 'U) : 'U =
        Unchecked.defaultof<'U>

    member this.Return (value : 'T) : 'T =
        value

    member this.Quote (expr : Expr<'T>) : Expr<Divinable<'T>> =
        obj () :?> Expr<Divinable<'T>>

    member this.Run<'T> (expr : Expr<'T>) : Divined<'T> =
        this.Quote (expr) |> QuotationEvaluator.Evaluate |> Divinable.divine diviner ({ DivineContext.Variables = Map.empty })
