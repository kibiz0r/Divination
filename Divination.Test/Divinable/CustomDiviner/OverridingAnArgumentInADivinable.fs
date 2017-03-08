namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Custom-divining a divinable's argument`` =
    module MyModule =
        let aFuncThatAcceptsAnArgumentAndReturnsDivinable (anArgument : int) =
            divinable {
                return anArgument + 2
            }

    [<Test>]
    let ``allows overriding arguments to the divinable`` () =
        let customReturnValue = 7
        let myDiviner = {
            new Diviner () with
                override this.Var<'T> (name, value, type') =
                    match name with
                    | "anArgument" ->
                        (5 :> obj) :?> 'T
                    | _ ->
                        base.Var<'T> (name, value, type')
            }
        let myDivined =
            (divinable {
                let myArgument = 1
                let! returnValue = MyModule.aFuncThatAcceptsAnArgumentAndReturnsDivinable myArgument
                return returnValue
            }).Divine (DivinationBinding.empty myDiviner)
        myDivined.Value |> should equal customReturnValue
