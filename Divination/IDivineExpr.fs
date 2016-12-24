namespace Divination

open System

type IDivineExpr =
    interface
    end

type IDivineExpr<'T> =
    inherit IDivineExpr
    abstract member Raw : IDivineExpr