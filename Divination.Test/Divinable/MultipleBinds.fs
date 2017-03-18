namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Multiple binds`` =
    [<Test>]
    let ``works`` () =
        let intAddition = <@ 1 + 1 @>.ToMethodInfo ()
        let myDivined : Divined<int> =
            FSharpDiviner.Current.Divine (IdentificationScope.empty (),
                divinable {
                    let! x = Divinable.value (5 :> obj, typeof<int>) : IDivinable<int>
                    let! y = Divinable.value (2 :> obj, typeof<int>) : IDivinable<int>
                    return x + y
                }
            )
        myDivined.Identity |> should equal (
            LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>),
                LetIdentity (VarIdentity "y", ValueIdentity (2 :> obj, typeof<int>),
                    CallIdentity (None, intAddition, [VarIdentity "x"; VarIdentity "y"])
                )
            ) : Identity)
        myDivined.Value |> should equal 7

