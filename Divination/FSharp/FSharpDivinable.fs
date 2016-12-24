namespace Divination.FSharp

open System
open Divination

type FSharpDivinable = {
    Expr : FSharpExpr
} with
    interface IDivinable with
        member this.DivineExpr () = this.Expr :> IDivineExpr

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpDivinable =
    let fromExpr (expr : FSharpExpr) : FSharpDivinable =
        { Expr = expr }

    let value (value : obj, type' : Type) : IDivinable =
        fromExpr (FSharpExpr.FSharpValue { Value = value; Type = type' }) :> IDivinable