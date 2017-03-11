namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Binds changing type`` =
    [<Test>]
    let ``works`` () =
        let intToString = typeof<int>.GetMethod ("ToString", [||])
        let myDivined : Divined<string> =
            (divinable {
                let! x = Divinable.value 5 : IDivinable<int>
                return x.ToString ()
            }).Divine (IdentificationScope.empty (), Diviner.Current)
        myDivined.Identity |> should equal (CallIdentity (Some (ValueIdentity (5 :> obj, typeof<int>)), intToString, []) : Identity)
        myDivined.Value |> should equal 5
