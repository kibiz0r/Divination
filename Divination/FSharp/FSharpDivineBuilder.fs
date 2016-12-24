namespace Divination.FSharp

open System
open Divination
open FSharp.Quotations
open FSharp.Quotations.Evaluator

type FSharpDivineBuilder (diviner : IFSharpDiviner, exalter : IFSharpExalter) =
    member this.Bind (divinable : IDivinable<'T>, f : 'T -> 'U) : 'U =
        Unchecked.defaultof<'U>

    member this.Return (value : 'T) : 'T =
        value

    member this.Quote (expr : Expr<'T>) : Expr<'T> =
        expr

    member this.Run<'T> (expr : Expr<'T>) : Divined<'T> =
        let divinable = Exalter.exalt exalter expr |> Divinable.cast
        let context = diviner.NewContext ()
        Divinable.divine diviner context divinable