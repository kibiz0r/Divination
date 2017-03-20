namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Simple return`` =
    [<Test>]
    let ``can be manually built via identities`` () =
        let myDivinable =
            Divinable<int> (DivinationContext.return' (ValueIdentity (5 :> obj, typeof<int>)))
        let myDivined = myDivinable.Divine ()
        myDivined.Identity |> should equal (ValueIdentity (5 :> obj, typeof<int>))
        myDivined.Value |> should equal 5

    [<Test>]
    let ``can be manually built via divinables`` () =
        let valueDivinable = Divinable.value 5
        let myDivinable =
            Divinable<int> (fun context ->
                context.Return (ValueIdentity (5 :> obj, typeof<int>))
            )
        let myDivined = myDivinable.Divine ()
        myDivined.Identity |> should equal (ValueIdentity (5 :> obj, typeof<int>))
        myDivined.Value |> should equal 5