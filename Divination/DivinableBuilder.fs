namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>) : IDivinable<'U> =
        Divinable<'U> (fun (diviner, binding) ->
            let sourceDivinable = QuotationEvaluator.Evaluate divinableExpr
            let sourceDivined = sourceDivinable.Divine (diviner, binding)
            let sourceValue = sourceDivined.Value
            let bodyDivinable = body sourceValue
            let bodyDivined = bodyDivinable.Divine (diviner, binding)
            bodyDivined
        ) :> IDivinable<'U>
        //Divinable.expr divinableExpr
        //|> Divinable.unwrap
        //|> Divinable.bind body

    member this.Return ([<ReflectedDefinition>] returnExpr : Expr<'T>) : IDivinable<'T> =
        Divinable<'T> (fun (diviner, binding) -> diviner.Resolve (returnExpr.ToIdentity ())) :> IDivinable<'T>