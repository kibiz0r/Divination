namespace Divination

open System
open FSharp.Quotations

type ExprDivinable<'T> (expr : Expr<'T>) =
    interface IDivinable<'T> with
        member this.Identity diviner =
            expr :> obj