namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<IDivinable<'T>>, [<ReflectedDefinition (true)>] exprWithBody : Expr<'T -> IDivinable<'U>>) : IDivinable<'U> =
        match exprWithBody with
        | WithValue ((:? ('T -> IDivinable<'U>) as body), _, (:? (Expr<'T -> IDivinable<'U>>) as bodyExpr)) ->
            this.DivinableFromDivinableExprAndBodyAndBodyExpr (divinableExpr, body, bodyExpr)
        | _ -> invalidOp ""

    member this.Return ([<ReflectedDefinition>] returnExpr : Expr<'T>) : IDivinable<'T> =
        Divinable<'T> (fun (diviner, scope) ->
            let identity = returnExpr.ToIdentity ()
            match IdentificationScope.tryFind identity scope with
            | Some i -> i
            | None -> identity
        ) :> IDivinable<'T>

    member this.Run ([<ReflectedDefinition (true)>] exprWithRun : Expr<IDivinable<'T>>) : IDivinable<'T> =
        match exprWithRun with
        | WithValue (:? IDivinable<'T> as run, _, runExpr) ->
            let scope = this.ScopeFromExpr runExpr
            Divinable.mergeScope scope run
        | _ -> invalidOp ""

    member this.ScopeFromExpr (expr : Expr) : IdentificationScope =
        let exprString = expr.ToString ()
        let myArgumentIdentity = VarIdentity ("myArgument", typeof<int>)
        IdentificationScope.empty ()
        |> IdentificationScope.add myArgumentIdentity (ValueIdentity (42 :> obj, typeof<int>))

    member this.DivinableFromDivinableExprAndBodyAndBodyExpr (divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>, bodyExpr : Expr<'T -> IDivinable<'U>>) : IDivinable<'U> =
        Divinable<'U> (fun (diviner, scope) ->
            let divinableIdentity = divinableExpr.ToIdentity ()
            let divinable : IDivinable<'U> =
                match divinableIdentity with
                | CallIdentity (this', methodInfo, arguments) ->
                    let parameterInfos = methodInfo.GetParameters ()
                    let scopeWithArguments = Seq.zip arguments parameterInfos |> Seq.fold (fun (s : IdentificationScope) (a, p) -> IdentificationScope.add (VarIdentity (p.Name, p.ParameterType)) a s) scope
                    diviner.ResolveValue (divinableIdentity, scopeWithArguments)
                | _ ->
                    diviner.ResolveValue (divinableIdentity, scope)
            let divinedValue = diviner.ResolveValue (divinable.Identify (diviner, scope), scope)
            let bodyDivinable = body divinedValue
            let bodyExprString = bodyExpr.ToString ()
            let bodyScopeWithArguments = scope |> IdentificationScope.add (VarIdentity ("returnValue", typeof<'T>)) (ValueIdentity (divinedValue :> obj, typeof<'T>))
            let bodyIdentity = bodyDivinable.Identify (diviner, bodyScopeWithArguments)
            bodyIdentity
        ) :> IDivinable<'U>