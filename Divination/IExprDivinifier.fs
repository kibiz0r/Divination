namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open FSharp.Quotations
open FSharp.Quotations.Patterns

type IExprDivinifier<'Identifier> =
    abstract member ToDivinableBase : Expr -> IDivinable<obj, 'Identifier>
    abstract member ToDivinableBase : Var -> IDivinable<obj, 'Identifier>
    abstract member ToDivinable<'T> : Expr<'T> -> IDivinable<'T, 'Identifier>
