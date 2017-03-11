namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Multiple binds`` =
    [<Test>]
    let ``works`` () =
        let intAddition = <@ 1 + 1 @>.ToMethodInfo ()
        let myDivined : Divined<int> =
            (divinable {
                let! x = Divinable.value 5 : IDivinable<int>
                let! y = Divinable.value 2 : IDivinable<int>
                return x + y
            }).Divine (IdentificationScope.empty (), Diviner.Current)
        myDivined.Identity |> should equal (CallIdentity (None, intAddition, [ValueIdentity (5 :> obj, typeof<int>); ValueIdentity (2 :> obj, typeof<int>)]) : Identity)
        myDivined.Value |> should equal 7

