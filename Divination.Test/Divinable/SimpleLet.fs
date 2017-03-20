namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Simple let`` =
    //[<Test>]
    //let ``can be divined`` () =
    //    let myDivined : Divined<int> =
    //        let divinable =
    //            divinable {
    //                let x = 5
    //                return x
    //            }
    //        FSharpDiviner.Current.Divine (IdentificationScope.empty (), divinable)
    //    myDivined.Value |> should equal 5

    [<Test>]
    let ``can be built via an identity`` () =
        let myDivinable =
            Divinable<int> (DivinationContext.return' (LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x")))
        let myDivined = myDivinable.Divine ()
        myDivined.Identity |> should equal (LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x"))
        myDivined.Value |> should equal 5

    [<Test>]
    let ``can be erased by building through nested identities`` () =
        //let var = Divinable.var "x"
        //let argument = Divinable.value 5
        //let body = Divinable.var "x"
        let var = VarIdentity "x"
        let argument = ValueIdentity (5 :> obj, typeof<int>)
        let body = VarIdentity "x"
        let myDivinable = Divinable<int> (fun context ->
            context |> DivinationContext.let' var argument |> DivinationContext.return' body
        )
        let myDivined = myDivinable.Divine ()
        myDivined.Identity |> should equal (ValueIdentity (5 :> obj, typeof<int>))
        myDivined.Value |> should equal 5

    //[<Test>]
    //let ``can rely on other divinables`` () =
    //    let myDivinable = Divinable.let' (Divinable.var "x") (Divinable.value 5) (Divinable.var "x")
    //    let myDivined = myDivinable.Divine ()
    //    myDivined.Identity |> should equal (LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x"))
    //    myDivined.Value |> should equal 5

    
    //[<Test>]
    //let ``Divinable.let' also works`` () =
    //    let myDivined : Divined<int> =
    //        let divinable = Divinable.let' (Divinable.var "x") (Divinable.value (5 :> obj, typeof<int>)) (Divinable.var "x")
    //        FSharpDiviner.Current.Divine (IdentificationScope.empty (), divinable)
    //    let expected : Identity = LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x")
    //    myDivined.Identity |> should equal expected
    //    myDivined.Value |> should equal 5