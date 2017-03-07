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
                return sprintf "%s: %i" "to override" anArgument
            }

    [<Test>]
    let ``allows manipulating the arguments and the return value`` () =
        let customString = "overriden return value"
        let myDiviner = {
            new Diviner () with
                override this.Value<'T> (value, _) =
                    match value with
                    | :? string as str ->
                        if str = "to override" then
                            (customString :> obj) :?> 'T
                        else
                            (str :> obj) :?> 'T
                    | other -> other :?> 'T
            }
        let myDiviner = Diviner ()
        let myDivined =
            (divinable {
                let argumentToDivinable = 5
                let! aThing = MyModule.aFuncThatReturnsDivinable argumentToDivinable
                return aThing
            }).Divine (myDiviner, DivinationBinding.empty ())
        myDivined.Value |> should equal customString