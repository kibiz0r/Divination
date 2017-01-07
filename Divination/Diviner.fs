namespace Divination

open System
open FSharp.Interop.Dynamic
open FSharp.Quotations
open FSharp.Quotations.Evaluator

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    let identify (diviner : IDiviner) (divinable : IDivinable<'T>) : obj =
        divinable.Identify diviner

    let divine (diviner : IDiviner) (identity : obj) : Divined<'T> =
        let value : 'T = diviner?Divine identity
        { Identity = identity; Value = value }

    let identifyAndDivine (diviner : IDiviner) (divinable : IDivinable<'T>) : Divined<'T> =
        identify diviner divinable |> divine diviner

    let identifyAndDivineValue (diviner : IDiviner) (divinable : IDivinable<'T>) : 'T =
        (identifyAndDivine diviner divinable).Value

type Diviner () =
    interface IDiviner

    member this.Divine<'T> (expr : Expr<'T>) : 'T =
        expr |> QuotationEvaluator.Evaluate

    member this.Divine<'T> (wrapped : IDivinable<IDivinable<'T>>) : 'T =
        wrapped |> Diviner.identifyAndDivineValue this |> Diviner.identifyAndDivineValue this

    //member this.Divine<'T> (unwrapDivinable : UnwrapDivinable<'T>) : 'T =
    //    Diviner.divine this unwrapDivinable.Wrapped |> Diviner.divine this

    //member this.Divine<'T, 'U> (callDivinable : CallDivinable<'T, 'U>) : 'U =
    //    Diviner.divine this callDivinable.Argument |> callDivinable.Body