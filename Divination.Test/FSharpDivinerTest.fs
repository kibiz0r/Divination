namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open Divination.FSharp
open FSharp.Quotations
open FSharp.Quotations.Evaluator
open FSharp.Interop.Dynamic

// So here's what we do:
// We start with a normal expression tree
// We end up with a Divinable tree, which represents an unevaluated expression tree that only consists of expressions that can be consistently replayed anywhere
// Divinables only consist of hard, primitive data
// When we evaluate the Divinable, we must provide a context for the expression tree, which comes in the form of a IDiviner
// Since we cannot know what sort of context the Divinable ultimately needs, nor what the IDiviner provides, we pass it off to the IDiviner according to its union case

[<TestFixture>]
module FSharpDivinerTest =
    let diviner = FSharpDiviner () :> IFSharpDiviner
    let context = FSharpDiviningContext () :> IFSharpDiviningContext
    let divine = Divinable.divine diviner context

    [<Test>]
    let ``FSharpDiviner does stuff`` () =
        let divinable = FSharpDivinable.value (5, typeof<int>) |> Divinable.cast
        let divined : Divined<int> = divine divinable
        let expected : Divined<int> = { Source = FSharpExpr.FSharpValue { Value = 5; Type = typeof<int> }; Value = 5 }
        divined |> should equal expected