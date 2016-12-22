namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open FSharp.Quotations

//type DerpClass () =
//    static member x = 1
//    static member y = 2

//module DerpModule =
//    let x = 3
//    let y = 4

//    let added z =
//        x + y + z

//    let useList ls =
//        ls |> Seq.map (fun l -> "Hello " + l)

[<TestFixture>]
module DivinedBuilderTest =
    [<Test>]
    let ``DivinedBuilder does something`` () =
        let divined : Divined<string> =
            divined {
                let hello = "Hello"
                let world = "World"
                return hello + " " + world
            }
        divined.Value |> should equal "Hello World"