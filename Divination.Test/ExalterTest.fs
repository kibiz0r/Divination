namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open FSharp.Quotations
open FSharp.Quotations.Evaluator

[<TestFixture>]
module ExalterTest =
    let exalter = Exalter () :> IExalter

    [<Test>]
    let ``Exalter does simple let-value transformation`` () =
        let exalted =
            exalter.Exalt
                <@
                    let x : int = 5
                    x
                @>
        let expected : IDivinable<int> =
            Divinable.let'
                (
                    Divinable.var ("x", typeof<int>),
                    Divinable.value (5, typeof<int>),
                    Divinable.varGet "x"
                )
            |> Divinable.cast
        exalted |> should equal expected

    type SpecialMethodClass () =
        [<ExaltedMethod("MyExaltedInterfaceMethod")>]
        static member MyProxyInterfaceMethod (x : int) : int =
            raise (ExaltedOnlyMethodException ())

        static member MyExaltedInterfaceMethod (x : IDivinable) : IDivinable =
            Divinable.call (None, "op_Addition", [x; Divinable.value (2, typeof<int>)])

        [<ExaltedMethod("MyExaltedTypedMethod")>]
        static member MyProxyTypedMethod (x : int) : int =
            raise (ExaltedOnlyMethodException ())

        static member MyExaltedTypedMethod (x : IDivinable<int>) : IDivinable<int> =
            Divinable.call (None, "op_Addition", [x; Divinable.value (2, typeof<int>)]) |> Divinable.cast

        [<ExaltedMethod("MyExaltedGenericMethod")>]
        static member MyProxyGenericMethod<'T> (x : 'T) : 'T =
            raise (ExaltedOnlyMethodException ())

        static member MyExaltedGenericMethod<'T> (x : IDivinable<'T>) : IDivinable<'T> =
            Divinable.call (None, "op_Addition", [x; Divinable.value (2, typeof<int>)]) |> Divinable.cast

    [<Test>]
    let ``Exalter lets you use special methods`` () =
        let exalted =
            exalter.Exalt
                <@
                    let x : int = 5
                    SpecialMethodClass.MyProxyInterfaceMethod x
                @>
        let expected : IDivinable<int> =
            Divinable.let'
                (
                    Divinable.var ("x", typeof<int>),
                    Divinable.value (5, typeof<int>),
                    SpecialMethodClass.MyExaltedInterfaceMethod (Divinable.varGet "x")
                )
            |> Divinable.cast
        exalted |> should equal expected

    [<Test>]
    let ``Exalter lets you use special typed methods`` () =
        let exalted =
            exalter.Exalt
                <@
                    let x : int = 5
                    SpecialMethodClass.MyProxyTypedMethod x
                @>
        let expected : IDivinable<int> =
            Divinable.let'
                (
                    Divinable.var ("x", typeof<int>),
                    Divinable.value (5, typeof<int>),
                    SpecialMethodClass.MyExaltedTypedMethod (Divinable.varGet "x" |> Divinable.cast)
                )
            |> Divinable.cast
        exalted |> should equal expected

    [<Test>]
    let ``Exalter does not let you use special generic methods`` () =
        (fun () ->
            exalter.Exalt
                <@
                    let x : int = 5
                    SpecialMethodClass.MyProxyGenericMethod x
                @> |> ignore
        ) |> should (throwWithMessage "Exalted methods must not be generic") typeof<InvalidOperationException>