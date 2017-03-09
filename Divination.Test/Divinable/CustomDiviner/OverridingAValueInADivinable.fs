namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Custom-divining a divinable's return value`` =
    module MyModule =
        let aFuncThatReturnsDivinable () =
            divinable {
                return "to override"
            }

    [<Test>]
    let ``allows overriding values within the divinable`` () =
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
        let myDivined =
            (divinable {
                let! returnValue = MyModule.aFuncThatReturnsDivinable ()
                return returnValue
            }).Divine (myDiviner, IdentificationScope.empty ())
        myDivined.Value |> should equal customString