namespace Divination

open System

module Diviner =
    let eval (diviner : IDiviner<'Expr, 'Context>) (expr : IDivineExpr) (context : 'Context) =
        diviner.Eval (expr :?> 'Expr, context)
