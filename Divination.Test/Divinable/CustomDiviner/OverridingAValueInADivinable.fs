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
            new FSharpDiviner () with
                override this.Value (scope, value, _) =
                    match value with
                    | :? string as str ->
                        if str = "to override" then
                            customString :> _
                        else
                            str :> _
                    | other -> other
            }
        let myDivined =
            FSharpDiviner.Current.Divine (DivinationScope.empty,
                divinable {
                    let! returnValue = MyModule.aFuncThatReturnsDivinable ()
                    return returnValue
                }
            )
        myDivined.Value |> should equal customString