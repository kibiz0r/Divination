namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Evaluator

type DivinableBuilder (exalter : IExalter) =
    member this.Bind (divinable : IDivinable<'T>, f : 'T -> 'U) : 'U =
        Unchecked.defaultof<'U>

    member this.Return (value : 'T) : 'T =
        value

    member this.Quote (expr : Expr<'T>) : Expr<'T> =
        expr

    member this.Run<'T> (expr : Expr<'T>) : IDivinable<'T> =
        exalter.Exalt expr