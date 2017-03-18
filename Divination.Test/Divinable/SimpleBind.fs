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
            FSharpDiviner.Current.Divine (IdentificationScope.empty (),
                divinable {
                    let! x = Divinable.value (5 :> obj, typeof<int>)
                    return x
                }
            )
        myDivined.Identity |> should equal (LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x") : Identity)
        myDivined.Value |> should equal 5
