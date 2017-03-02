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
    let divineValue divinable =
        Diviner.value (FSharpDiviner ()) divinable

    [<SetUp>]
    let setUp () =
        MyModule.myNum <- 1

    [<Test>]
    let ``divinable does stuff`` () =
        let divinable =
            divinable {
                return 5
            }
        divineValue divinable |> should equal 5

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
        divineValue divinable1 |> should equal 2
        MyModule.myNum <- 2
        let divinable2 =
            divinable {
                let! x = MyModule.getADivinable MyModule.myNum
                return x
            }
        divineValue divinable1 |> should equal 4
        divineValue divinable2 |> should equal 4

    [<Test>]
    let ``divinable doesn't break when using simple lets`` () =
        let divinable1 =
            divinable {
                let! x = MyModule.getALetDivinable ()
                return x
            }
        divineValue divinable1 |> should equal 1
        MyModule.myNum <- 2
        let divinable2 =
            divinable {
                let! x = MyModule.getALetDivinable ()
                return x
            }
        divineValue divinable1 |> should equal 2
        divineValue divinable2 |> should equal 2

    [<Test>]
    let ``divinable doesn't break when using lets on calls`` () =
        let divinable1 =
            divinable {
                let! x = MyModule.getALetCallDivinable ()
                return x
            }
        divineValue divinable1 |> should equal 1
        MyModule.myNum <- 2
        let divinable2 =
            divinable {
                let! x = MyModule.getALetCallDivinable ()
                return x
            }
        divineValue divinable1 |> should equal 2
        divineValue divinable2 |> should equal 2

    [<Test>]
    let ``Maybe it should work like this?`` () =
        divinable {
            let ``value identified as return value of func`` : string = SomeModule.``func called with constant`` 5

            let ``value identified as new object with argument chained off of previous identity`` : SomeObject =
                SomeObject ``value identified as return value of func``

            let ``value identified as property of previous object`` : float =
                ``value identified as new object with argument chained off of previous identity``.SomeProperty

            let! ``value acquired by applying same diviner applied to this divinable, identified as computed by that diviner`` : int =
                SomeModule.``func called with normal values but returning an IDivinable`` : IDivinable<int>

            // And, conversely, maybe it can't work like this? --v

            let! ``Divined<'T> reduced to a 'T, but has its Identity tracked in the background...`` : int =
                ``some func that may not even be identifiable`` () : Divined<int>
                // ...but Divineds that were obtained from outside sources do not necessarily have accurate identities
                // according to this divinable block

            let! ``Identity evaluated into a 'T, using this current diviner and binding...`` : bool =
                ``some other not-necessarily-identifiable func`` () : Identity
                // ...but that creates some weird cognitive dissonance here, because unless the identity is only based
                // on global components (nothing within this chain of divinables) or only *accidentally* based on
                // components of this chain of divinables, there's no saying it will be able to be evaluated reliably,
                // or be a stable foundation for further chains... really the same problem as above, except that here
                // the value may not be computable, whereas above the value may differ from what would be computed inline
        }
        // It really seems like there are two things that Divination are trying to accomplish:
        //   1. Execute normal code, where variables are tracked by identity in the background, and optionally made
        //      available outside of the scope.
        //   2. Materialize Identities into concrete values of a specific type, within a given context.
        ()