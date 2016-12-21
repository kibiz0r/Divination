namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open FSharp.Quotations
open FSharp.Quotations.Evaluator

[<TestFixture>]
module ExalterTest =
    let exalter = Exalter () :> IExalter

    [<Test>]
    let ``Exalter does stuff`` () =
        let exalted =
            exalter.Exalt
                <@
                    let x : int = 5
                    x
                @>
        let expected : IDivinable<int> =
            Divinable.let'
                (
                    Divinable.var ("x", typeof<int>),
                    Divinable.value (5, typeof<int>),
                    Divinable.varGet ("x")
                )
            |> Divinable.cast
        exalted |> should equal expected