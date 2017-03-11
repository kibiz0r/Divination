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
        let objToString = <@ (obj ()).ToString () @>.ToMethodInfo ()
        let myDivined : Divined<string> =
            (divinable {
                let! x = Divinable.value 5 : IDivinable<int>
                return x.ToString ()
            }).Divine (IdentificationScope.empty (), Diviner.Current)
        let expected = (CallIdentity (Some (ValueIdentity (5 :> obj, typeof<int>)), objToString, []) : Identity)
        myDivined.Identity |> should equal expected
        myDivined.Value |> should equal "5"
