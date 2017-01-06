namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>) : IDivinable<'U> =
        Divinable.expr divinableExpr
        |> Divinable.unwrap
        |> Divinable.bind body

    member this.Return ([<ReflectedDefinition>] valueExpr : Expr<'T>) : IDivinable<'T> =
        Divinable.expr valueExpr