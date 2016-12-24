namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open Divination.FSharp

[<TestFixture>]
module FSharpDivineBuilderTest =
    [<Test>]
    let ``divine does stuff`` () =
        let divined =
            divine {
                return 5
            }
        divined.Value |> should equal 5