namespace Divination

open System
open FSharp.Interop.Dynamic
open FSharp.Quotations
open FSharp.Quotations.Evaluator

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    let identify (diviner : IDiviner) (divinable : IDivinable<'T>) : obj =
        divinable.Identity diviner

    let divine (diviner : IDiviner) (divinable : IDivinable<'T>) : 'T =
        diviner?Divine (identify diviner divinable)

type Diviner () =
    interface IDiviner

    member this.Divine<'T> (expr : Expr<'T>) : 'T =
        expr |> QuotationEvaluator.Evaluate

    member this.Divine<'T> (wrapped : IDivinable<IDivinable<'T>>) : 'T =
        wrapped |> Diviner.divine this |> Diviner.divine this

    //member this.Divine<'T> (unwrapDivinable : UnwrapDivinable<'T>) : 'T =
    //    Diviner.divine this unwrapDivinable.Wrapped |> Diviner.divine this

    //member this.Divine<'T, 'U> (callDivinable : CallDivinable<'T, 'U>) : 'U =
    //    Diviner.divine this callDivinable.Argument |> callDivinable.Body