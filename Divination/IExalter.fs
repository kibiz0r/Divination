namespace Divination

open System
open System.Reflection
open FSharp.Quotations

/// <summary>Promotes a plain Expr into an elevated expression of a custom type.</summary>
type IExalter<'Exalted, 'Primitive, 'Type, 'MethodInfo, 'Var, 'Expr when 'Exalted :> IDivinable and 'Var :> IDivineVar and 'Expr :> IDivineExpr> =
    abstract member Exalt : 'Expr -> 'Exalted

    abstract member Primitive : obj * 'Type -> 'Primitive
    abstract member Type : Type -> 'Type
    abstract member MethodInfo : MethodInfo -> 'MethodInfo
    abstract member Var : Var -> 'Var

    abstract member Call : 'Expr option * 'MethodInfo * 'Expr list -> 'Expr
    abstract member Let : 'Var * 'Expr * 'Expr -> 'Expr
    abstract member Value : 'Primitive * 'Type -> 'Expr
    abstract member VarGet : 'Var -> 'Expr