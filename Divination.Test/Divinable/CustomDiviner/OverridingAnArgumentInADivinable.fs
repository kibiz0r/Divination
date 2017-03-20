namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination

// Who is responsible for ensuring that identities get mapped through the scope/context?
// That's probably too much power for divinables, but if diviners have it as part of their public interface then
// divinables implicitly have that power...
// Unless divinables don't get a reference to the diviner?
// Instead, diviners give divinables a sandbox in which to operate?
// Hm... Well, on the other end of this problem, diviners definitely need the ability to control how identities are
// interpreted
// Oh hey, what if the sandbox (call it a "context"?) is able to turn identities into divineds, and the signature of
// IDivinable.Identify returns a Divined?
// Eh, actually it seems fine as-is... Divinables kinda *have* to cooperate with the diviner's context system, in order
// to be able to shim in things like DivinableBuilder.Bind ()... but it's probably a good idea to make the context type
// generic and agreed-upon by the divinable and diviner, both, so it can be any kind of object

// In a situation like...
// 
// let! x =
//     let y = 5
//     makeADivinable y
// 
// ...turn the whole right-hand side into a divinable of a divinable, where the scope of the outer divinable is kept,
// but the identity of the inner divinable is swapped with the computed identity. When computing the identity of the
// inner divinable, create a scope that maps the arguments of the inner expression to the identities in the outer scope

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
            new FSharpDiviner () with
                override this.Var (scope, name) =
                    match name with
                    | "anArgument" ->
                        5 :> _
                    | _ ->
                        base.Var (scope, name)
            }
        let myDivined =
            FSharpDiviner.Current.Divine (DivinationScope.empty,
                divinable {
                    let myArgument = 1
                    let! returnValue = MyModule.aFuncThatAcceptsAnArgumentAndReturnsDivinable myArgument
                    let someValue = "foo"
                    return returnValue
                }
            )
        myDivined.Value |> should equal customReturnValue
