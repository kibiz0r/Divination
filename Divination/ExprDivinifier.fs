namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open FSharp.Quotations
open FSharp.Quotations.Patterns

type ExprDivinifier<'Identifier> (?interceptor : Expr -> IDivinable<obj, 'Identifier> option) =
    interface IExprDivinifier<'Identifier> with
        member this.ToDivinableBase (expr : Expr) : IDivinable<obj, 'Identifier> =
            Unchecked.defaultof<IDivinable<obj, 'Identifier>>

        member this.ToDivinableBase (var : Var) : IDivinable<obj, 'Identifier> =
            Unchecked.defaultof<IDivinable<obj, 'Identifier>>

        member this.ToDivinable<'T> (expr : Expr<'T>) : IDivinable<'T, 'Identifier> =
            Unchecked.defaultof<IDivinable<'T, 'Identifier>>