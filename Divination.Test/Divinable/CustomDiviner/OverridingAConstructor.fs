namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Custom-divining a constructor`` =
    [<Test>]
    let ``allows replacing with something else entirely`` () =
        let customOverride : obj = "this is my new return value" :> obj
        let myDiviner = {
            new FSharpDiviner () with
                override this.NewObject (scope, constructorInfo, arguments) =
                    customOverride
            }
        let myDivined =
            FSharpDiviner.Current.Divine (DivinationScope.empty,
                divinable {
                    return obj ()
                }
            )
        myDivined.Value |> should equal customOverride