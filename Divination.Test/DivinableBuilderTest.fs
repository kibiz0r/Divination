namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

module MyModule =
    let mutable myNum = 1

    let getADivinable (n) =
        divinable {
            return myNum + n
        }

    let getALetDivinable () =
        divinable {
            let x = myNum
            return x
        }

    let myCall () =
        myNum

    let getALetCallDivinable () =
        divinable {
            let x = myCall ()
            return x
        }

[<TestFixture>]
module DivinableBuilderTest =
    let divine divinable =
        Diviner.divine (Diviner ()) divinable

    [<SetUp>]
    let setUp () =
        MyModule.myNum <- 1

    [<Test>]
    let ``divinable does stuff`` () =
        let divinable =
            divinable {
                return 5
            }
        divine divinable |> should equal 5

    [<Test>]
    let ``divinable does further stuff`` () =
        let divinable1 =
            divinable {
                let! x =
                    let theNum = MyModule.myNum
                    MyModule.getADivinable theNum
                return (
                    let r = x
                    r
                )
            }
        divine divinable1 |> should equal 2
        MyModule.myNum <- 2
        let divinable2 =
            divinable {
                let! x = MyModule.getADivinable MyModule.myNum
                return x
            }
        divine divinable1 |> should equal 4
        divine divinable2 |> should equal 4

    [<Test>]
    let ``divinable doesn't break when using simple lets`` () =
        let divinable1 =
            divinable {
                let! x = MyModule.getALetDivinable ()
                return x
            }
        divine divinable1 |> should equal 1
        MyModule.myNum <- 2
        let divinable2 =
            divinable {
                let! x = MyModule.getALetDivinable ()
                return x
            }
        divine divinable1 |> should equal 2
        divine divinable2 |> should equal 2

    [<Test>]
    let ``divinable doesn't break when using lets on calls`` () =
        let divinable1 =
            divinable {
                let! x = MyModule.getALetCallDivinable ()
                return x
            }
        divine divinable1 |> should equal 1
        MyModule.myNum <- 2
        let divinable2 =
            divinable {
                let! x = MyModule.getALetCallDivinable ()
                return x
            }
        divine divinable1 |> should equal 2
        divine divinable2 |> should equal 2