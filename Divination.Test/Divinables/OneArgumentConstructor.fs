namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

type OneArgumentConstructorType (str : string) =
    member val Str = str

[<TestFixture>]
module ``One-argument constructors`` =
    [<Test>]
    let ``divinable invokes constructor`` () =
        let myDivined =
            (divinable {
                return OneArgumentConstructorType "hello"
            }).Divine (Diviner.Current, DivinationBinding.empty ())
        myDivined.Value |> should be instanceOfType<OneArgumentConstructorType>
        myDivined.Value.Str |> should equal "hello"
