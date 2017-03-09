namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``One-argument constructors`` =
    [<Test>]
    let ``divinable invokes constructor`` () =
        let constructions = OneArgumentConstructorType.Constructed |> Observable.replay
        use __ = constructions.Connect ()

        let myDivined =
            (divinable {
                return OneArgumentConstructorType "hello"
            }).Divine (Diviner.Current, IdentificationScope.empty ())

        myDivined.Value |> should be instanceOfType<OneArgumentConstructorType>
        myDivined.Value.Str |> should equal "hello"
        constructions |> Observable.toConnectedList |> should equal [()]
