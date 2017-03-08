namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Custom-divining a divinable`` =
    module MyModule =
        let aFuncThatReturnsDivinable (anArgument : int) =
            divinable {
                let str = anArgument.ToString ()
                return "to override" + str
            }

    [<Test>]
    let ``allows manipulating the arguments and the return value`` () =
        let customString = "overriden return value: "
        let myDiviner = {
            new Diviner () with
                override this.Value<'T> (binding, value, _) =
                    match value with
                    | :? string as str ->
                        if str = "to override" then
                            (customString :> obj) :?> 'T
                        else
                            (str :> obj) :?> 'T
                    | :? int as i ->
                        (7 :> obj) :?> 'T
                    | other -> other :?> 'T
            }
        let myDivined =
            (divinable {
                let argumentToDivinable = 5
                let! aThing = MyModule.aFuncThatReturnsDivinable argumentToDivinable
                return aThing
            }).Divine (myDiviner, DivinationBinding.empty ())
        myDivined.Value |> should equal (customString + "7")