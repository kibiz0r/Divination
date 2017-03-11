namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Simple let`` =
    [<Test>]
    let ``can be divined`` () =
        let myDivined : Divined<int> =
            (divinable {
                let x = 5
                return x
            }).Divine (IdentificationScope.empty (), Diviner.Current)
        myDivined.Value |> should equal 5

    [<Test>]
    let ``Divinable.let' acts the same way as through DivinableBuilder`` () =
        let myDivined : Divined<int> =
            let divinable = Divinable.let' (Divinable.var "x") (Divinable.value 5) (Divinable.var "x")
            divinable.Divine (IdentificationScope.empty (), Diviner.Current)
        let expected : Identity = LetIdentity (VarIdentity ("x", typeof<int>), ValueIdentity (5 :> obj, typeof<int>), VarIdentity ("x", typeof<int>))
        myDivined.Identity |> should equal expected
        myDivined.Value |> should equal 5