namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Evaluator

module ``DivinableBuilder is dead`` =
    let x = 5
//type DivinableBuilder (exalter : IFSharpExalter) =
//    member this.Bind (divinable : IDivinable<'T>, f : 'T -> 'U) : 'U =
//        Unchecked.defaultof<'U>

//    member this.Return (value : 'T) : 'T =
//        value

//    member this.Quote (expr : Expr<'T>) : Expr<'T> =
//        expr

//    member this.Run<'T> (expr : Expr<'T>) : IFSharpExpr<'T> =
//        Exalter.exalt exalter expr |> FSharpExpr.cast