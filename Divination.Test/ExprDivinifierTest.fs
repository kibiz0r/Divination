namespace Divination.Test

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open NUnit.Framework
open FsUnit
open Divination

#nowarn "40"

[<TestFixture>]
module ``ExprDivinifier`` =
    [<Test>]
    let ``divinifies Exprs normally`` () =
        let exprDivinifier = ExprDivinifier () :> IExprDivinifier
        let identity =
            exprDivinifier.ToDivinable(
                <@
                let x = 5
                x
                @>).Identify (IdentificationScope.empty (), Diviner.Current)
        identity |> should equal (LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x") : Identity)

    type FakeBuilder () =
        static member Return (i : int) : string =
            invalidOp "was not intercepted"

    // I need some way to recursively walk an Expr, turning each node into basically a Divinable;
    // the thing about tracking these identities from the outside-in is that
    // I need a method that takes an Expr, converts it into a partial identity, but at the point where it hits a dynamic
    // node such as a Divinable call, it needs to yield the current scope and the remaining Expr tree, which then the
    // Divinable can use to 
    [<Test>]
    let ``allows intercepting Return`` () =
        let returnMethodInfo = <@ FakeBuilder.Return 0 @>.ToMethodInfo ()
        let rec exprDivinifier : IExprDivinifier =
            ExprDivinifier (
                fun (expr : Expr) ->
                    match expr with
                    | Call (None, returnMethodInfo, [argumentExpr]) ->
                        Some ((exprDivinifier :> IExprDivinifierBase).ToDivinableBase argumentExpr)
                    | _ -> None
                ) :> _
        let identity =
            exprDivinifier.ToDivinable(
                <@
                let x = 5
                FakeBuilder.Return x
                @>).Identify (IdentificationScope.empty (), Diviner.Current)
        identity |> should equal (LetIdentity (VarIdentity "x", ValueIdentity (5 :> obj, typeof<int>), VarIdentity "x") : Identity)