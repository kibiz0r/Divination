namespace Divination

open System
open FSharp.Quotations

module Divinable =
    let expr (expr : Expr<'T>) : IDivinable<'T> =
        ExprDivinable<'T> expr :> IDivinable<'T>

    let unwrap (wrapped : IDivinable<IDivinable<'T>>) : IDivinable<'T> =
        UnwrapDivinable wrapped :> IDivinable<'T>

    let bind (body : 'T -> IDivinable<'U>) (argument : IDivinable<'T>) : IDivinable<'U> =
        BindDivinable (body, argument) :> IDivinable<'U>
