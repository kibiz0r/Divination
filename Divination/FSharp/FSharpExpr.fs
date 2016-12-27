namespace Divination.FSharp

open System
open System.Reflection
open Divination
open FSharp.Quotations

[<CustomEquality; CustomComparison>]
type FSharpVar = {
    Var : Var
} with
    interface IDivineVar

    interface IComparable with
        member this.CompareTo other =
              match other with
              | :? FSharpVar as o -> compare (this.Var.Name, this.Var.Type.AssemblyQualifiedName, this.Var.IsMutable) (o.Var.Name, o.Var.Type.AssemblyQualifiedName, o.Var.IsMutable)
              | _ -> invalidArg "other" "cannot compare values of different types"

    override this.Equals other =
        match other with
        | :? FSharpVar as o -> (this.Var.Name, this.Var.Type, this.Var.IsMutable) = (o.Var.Name, o.Var.Type, o.Var.IsMutable)
        | _ -> false

    override this.GetHashCode () =
        hash (this.Var.Name, this.Var.Type, this.Var.IsMutable)

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