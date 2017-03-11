namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Simple bind`` =
    [<Test>]
    let ``works`` () =
        let myDivined : Divined<int> =
            (divinable {
                let! x = Divinable.value 5 : IDivinable<int>
                return x
            }).Divine (IdentificationScope.empty (), Diviner.Current)
        myDivined.Identity |> should equal (ValueIdentity (5 :> obj, typeof<int>) : Identity)
        myDivined.Value |> should equal 5
