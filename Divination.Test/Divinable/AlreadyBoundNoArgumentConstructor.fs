namespace Divination.Test

open System
open FSharp.Control.Reactive
open NUnit.Framework
open FsUnit
open Divination

[<TestFixture>]
module ``Already-bound no-argument constructors`` =
    [<Test; Ignore ("I'm not sure this feature is supported anymore")>]
    let ``divinable does not invoke constructor when already bound`` () =
        let myAlreadyConstructed = NoArgumentConstructorType ()

        let constructions = NoArgumentConstructorType.Constructed |> Observable.replay
        use __ = constructions.Connect ()

        let myScope =
            let scope = IdentificationScope.empty ()
            scope
            //let constructorIdentity = <@ NoArgumentConstructorType () @>.ToIdentity ()
            //IdentificationScope.add constructorIdentity someSpecialIdentityISuppose? scope

        let myDivined =
            (divinable {
                return NoArgumentConstructorType ()
            }).Divine (myScope, Diviner.Current)

        constructions |> Observable.toConnectedList |> should equal []
        myDivined.Value |> should equal myAlreadyConstructed
