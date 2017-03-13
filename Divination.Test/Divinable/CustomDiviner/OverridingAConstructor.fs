namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Custom-divining a constructor`` =
    [<Test>]
    let ``allows replacing with something else entirely`` () =
        let customOverride : obj = "this is actually an object" :> obj
        let myDiviner = {
            new Diviner () with
                override this.NewObject<'T> (scope, constructorInfo, arguments) =
                    customOverride :?> 'T
            }
        let myDivined =
            (divinable {
                return obj ()
            }).Divine (IdentificationScope.empty (), myDiviner)
        myDivined.Value |> should equal customOverride