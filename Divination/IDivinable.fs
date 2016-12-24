namespace Divination

open System

type IDivinable =
    abstract member DivineExpr : unit -> IDivineExpr

type IDivinable<'T> =
    inherit IDivinable
    abstract member Raw : IDivinable