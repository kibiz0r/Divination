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

        let myDivined : Divined<OneArgumentConstructorType> =
            FSharpDiviner.Current.Divine (IdentificationScope.empty (),
                divinable {
                    return OneArgumentConstructorType "hello"
                }
            )

        myDivined.Value |> should be instanceOfType<OneArgumentConstructorType>
        myDivined.Value.Str |> should equal "hello"
        constructions |> Observable.toConnectedList |> should equal [()]
