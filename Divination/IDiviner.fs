namespace Divination

open System
    
type IDiviner<'Expr, 'Context when 'Expr :> IDivineExpr and 'Context :> IDiviningContext> =
    abstract member NewContext : unit -> 'Context
    abstract member Eval : 'Expr * 'Context -> obj
