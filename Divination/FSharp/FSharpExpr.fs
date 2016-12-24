namespace Divination.FSharp

open System
open System.Reflection
open Divination
open FSharp.Quotations

[<StructuralEquality; StructuralComparison>]
type FSharpVar = {
    Var : Var
} with
    interface IDivineVar

type FSharpCall = {
    This : IDivineExpr option
    MethodInfo : MethodInfo
    Arguments : IDivineExpr list
}

type FSharpLet = {
    Var : IDivineVar
    Value : IDivineExpr
    Body : IDivineExpr
}

type FSharpValue = {
    Value : obj
    Type : Type
}

type FSharpVarGet = {
    Var : IDivineVar
}

type FSharpExpr =
    | FSharpCall of FSharpCall
    | FSharpLet of FSharpLet
    | FSharpValue of FSharpValue
    | FSharpVarGet of FSharpVarGet
    with
        interface IDivineExpr

type FSharpExpr<'T> = {
    Raw : FSharpExpr
} with
    interface IDivineExpr<'T> with
        member this.Raw = this.Raw :> IDivineExpr

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpExpr =
    let cast (raw : FSharpExpr) : FSharpExpr<'T> =
        { Raw = raw }

    let let' (var : IDivineVar, value : IDivineExpr, body : IDivineExpr) : FSharpExpr =
        FSharpLet { Var = var; Value = value; Body = body }

    let value (value : obj, type' : Type) : FSharpExpr =
        FSharpValue { Value = value; Type = type' }

    let var (var : Var) : FSharpVar =
        { Var = var }

    let varGet (var : IDivineVar) : FSharpExpr =
        FSharpVarGet { Var = var }