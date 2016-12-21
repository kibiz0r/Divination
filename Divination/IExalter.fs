namespace Divination

open System
open FSharp.Quotations

/// <summary>Promotes a plain Expr into an IDivinable.</summary>
type IExalter =
    abstract member Exalt<'T> : Expr<'T> -> Divinable<'T>