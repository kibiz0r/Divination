namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``No-argument constructors`` =
    [<Test>]
    let ``divinable invokes constructor`` () =
        let constructions = NoArgumentConstructorType.Constructed |> Observable.replay
        use __ = constructions.Connect ()

        let myDivined =
            (divinable {
                return NoArgumentConstructorType ()
            }).Divine (Diviner.Current, IdentificationScope.empty ())

        myDivined.Value |> should be instanceOfType<NoArgumentConstructorType>
        constructions |> Observable.toConnectedList |> should equal [()]