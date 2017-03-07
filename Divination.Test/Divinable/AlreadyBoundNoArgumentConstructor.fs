namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Already-bound no-argument constructors`` =
    [<Test>]
    let ``divinable does not invoke constructor when already bound`` () =
        let myAlreadyConstructed = NoArgumentConstructorType ()

        let constructions = NoArgumentConstructorType.Constructed |> Observable.replay
        use __ = constructions.Connect ()

        let myBinding =
            let binding = DivinationBinding.empty ()
            let constructorIdentity = <@ NoArgumentConstructorType () @>.ToIdentity ()
            binding.Set (constructorIdentity, DivinedValue (constructorIdentity, myAlreadyConstructed))

        let myDivined =
            (divinable {
                return NoArgumentConstructorType ()
            }).Divine (Diviner.Current, myBinding)

        constructions |> Observable.toConnectedList |> should equal []
        myDivined.Value |> should equal myAlreadyConstructed
