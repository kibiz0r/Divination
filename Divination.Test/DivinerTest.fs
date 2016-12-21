namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open FSharp.Quotations
open FSharp.Quotations.Evaluator
open FSharp.Interop.Dynamic

// So here's what we do:
// We start with a normal expression tree
// We end up with an opaque IDivinable<'T>, which represents an unevaluated expression tree that only consists of expressions that can be consistently replayed anywhere
// When we evaluate the IDivinable<'T>, we must provide a context for the expression tree, which comes in the form of a IDiviner
// Since we cannot know what sort of context the IDivinable<'T> needs, nor what the IDiviner provides, we do runtime resolution of overloads of Divine() on the diviner

[<TestFixture>]
module DivinerTest =
    let diviner = Diviner () :> IDiviner
    let divine (divinable : IDivinable) : Divined<'T> = Divinable.divine diviner divinable

    [<Test>]
    let ``Diviner does stuff`` () =
        let divinable = Divinable.value (5, typeof<int>)
        let divined : Divined<int> = divine divinable
        divined |> should equal { Source = divinable; Value = 5 }

    [<Test>]
    let ``Playground`` () =
        let divinable = { DivinableValueUnionCaseType.Value = 5 }
        let divinedValue : obj = diviner?Divine divinable
        divinedValue |> should equal 5