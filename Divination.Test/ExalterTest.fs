namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open FSharp.Quotations
open FSharp.Quotations.Patterns
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
                    Divinable.var ("x", typeof<int>.AssemblyQualifiedName),
                    Divinable.value (5, typeof<int>.AssemblyQualifiedName),
                    Divinable.varGet (Divinable.var ("x", typeof<int>.AssemblyQualifiedName))
                )
            |> Divinable.cast
        exalted |> should equal expected

    let operatorsTypeName =
        let methodInfo =
            match <@ 1 + 2 @> with
            | Call (_, m, _) -> m
            | _ -> null
        methodInfo.DeclaringType.AssemblyQualifiedName

    type SpecialMethodClass () =
        [<ExaltedMethod("MyExaltedInterfaceMethod")>]
        static member MyProxyInterfaceMethod (x : int) : int =
            raise (ExaltedOnlyMethodException ())

        static member MyExaltedInterfaceMethod (x : IDivinable) : IDivinable =
            Divinable.call (None, operatorsTypeName, "op_Addition", [x; Divinable.value (2, typeof<int>.AssemblyQualifiedName)])

        [<ExaltedMethod("MyExaltedTypedMethod")>]
        static member MyProxyTypedMethod (x : int) : int =
            raise (ExaltedOnlyMethodException ())

        static member MyExaltedTypedMethod (x : IDivinable<int>) : IDivinable<int> =
            Divinable.call (None, operatorsTypeName, "op_Addition", [x; Divinable.value (2, typeof<int>.AssemblyQualifiedName)]) |> Divinable.cast

        [<ExaltedMethod("MyExaltedGenericMethod")>]
        static member MyProxyGenericMethod<'T> (x : 'T) : 'T =
            raise (ExaltedOnlyMethodException ())

        static member MyExaltedGenericMethod<'T> (x : IDivinable<'T>) : IDivinable<'T> =
            Divinable.call (None, operatorsTypeName, "op_Addition", [x; Divinable.value (2, typeof<int>.AssemblyQualifiedName)]) |> Divinable.cast

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
                    Divinable.var ("x", typeof<int>.AssemblyQualifiedName),
                    Divinable.value (5, typeof<int>.AssemblyQualifiedName),
                    SpecialMethodClass.MyExaltedInterfaceMethod (Divinable.varGet (Divinable.var ("x", typeof<int>.AssemblyQualifiedName)))
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
                    Divinable.var ("x", typeof<int>.AssemblyQualifiedName),
                    Divinable.value (5, typeof<int>.AssemblyQualifiedName),
                    SpecialMethodClass.MyExaltedTypedMethod (Divinable.varGet (Divinable.var ("x", typeof<int>.AssemblyQualifiedName)) |> Divinable.cast)
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