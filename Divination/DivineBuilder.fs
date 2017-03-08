namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

// DivineBuilder wraps a DivinableBuilder and invokes it with the current Diviner and an empty DivinationBinding
type DivineBuilder (divinableBuilder : DivinableBuilder) =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>) : IDivinable<'U> =
        obj () :?> IDivinable<'U>
        //Divinable.expr divinableExpr
        //|> Divinable.unwrap
        //|> Divinable.bind body

    member this.Return ([<ReflectedDefinition>] returnExpr : Expr<'T>) : IDivinable<'T> =
        Divinable<'T> (fun (diviner, binding) -> diviner.Resolve (binding, returnExpr.ToIdentity ())) :> IDivinable<'T>
