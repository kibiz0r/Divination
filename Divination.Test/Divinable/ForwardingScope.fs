namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Forwarding scope`` =
    let divinableCall (x : int) : IDivinable<int> =
        Divinable<int> (fun (scope, diviner) ->
            VarIdentity "x"
        ) :> _

    [<Test>]
    let ``works`` () =
        Divinable<int> (fun (scope, diviner) ->
            let scope = IdentificationScope.add (VarIdentity "x")
        ) :> _
        Divinable.bind 
        let objToString = <@ (obj ()).ToString () @>.ToMethodInfo ()
        let myDivined : Divined<string> =
            (divinable {
                let! x = Divinable.value 5 : IDivinable<int>
                return x.ToString ()
            }).Divine (IdentificationScope.empty (), Diviner.Current)
        let expected = (
            LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>),
                CallIdentity (Some (VarIdentity "x"), objToString, [])
            ) : Identity)
        myDivined.Identity |> should equal expected
        myDivined.Value |> should equal "5"

