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
            let divinable =
                divinable {
                    let x = 5
                    return x
                }
            FSharpDiviner.Current.Divine (IdentificationScope.empty (), divinable)
        myDivined.Value |> should equal 5

    [<Test>]
    let ``Divinable.let' acts the same way as through DivinableBuilder`` () =
        let myDivined : Divined<int> =
            let divinable = Divinable.let' (Divinable.var "x") (Divinable.value (5 :> obj, typeof<int>)) (Divinable.var "x")
            FSharpDiviner.Current.Divine (IdentificationScope.empty (), divinable)
        let expected : Identity = LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x")
        myDivined.Identity |> should equal expected
        myDivined.Value |> should equal 5

    module HowToBind =
        let (var : Identity) (argument : IDivinable<IDivinable<'T>>) (body : IDivinable<'U>) : IDivinable<'U> =
            ()