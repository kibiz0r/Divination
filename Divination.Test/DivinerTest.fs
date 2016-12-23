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
// We end up with a Divinable tree, which represents an unevaluated expression tree that only consists of expressions that can be consistently replayed anywhere
// Divinables only consist of hard, primitive data
// When we evaluate the Divinable, we must provide a context for the expression tree, which comes in the form of a IDiviner
// Since we cannot know what sort of context the Divinable ultimately needs, nor what the IDiviner provides, we pass it off to the IDiviner according to its union case

[<TestFixture>]
module DivinerTest =
    let diviner = Diviner () :> IDiviner<_>
    let context = DiviningContext () :> IDiviningContext
    let divine = Divinable.divine diviner context

    [<Test>]
    let ``Diviner does stuff`` () =
        let divinable = Divinable.value (5, typeof<int>.FullName) |> Divinable.cast
        let divined : Divined<int> = divine divinable
        let expected : Divined<int> = { Source = DivinedExpr.DivinedValue { Value = 5; TypeName = typeof<int>.FullName }; Value = 5 }
        divined |> should equal expected