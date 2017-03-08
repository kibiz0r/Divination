namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>) : IDivinable<'U> =
        Divinable<'U> (fun (diviner, binding) ->
            let divinableIdentity = divinableExpr.ToIdentity ()
            let divinable : IDivinable<'T> = diviner.ResolveValue (binding, divinableIdentity)
            let divinedValue = (divinable.Divine (diviner, binding)).Value
            let bodyDivinable = body divinedValue
            let bodyDivined = bodyDivinable.Divine (diviner, binding)
            bodyDivined
        ) :> IDivinable<'U>

    member this.Return ([<ReflectedDefinition>] returnExpr : Expr<'T>) : IDivinable<'T> =
        Divinable<'T> (fun (diviner, binding) -> diviner.Resolve (binding, returnExpr.ToIdentity ())) :> IDivinable<'T>