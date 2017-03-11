namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () as this =
    let returnMethodDefinition =
        match <@ this.Return<obj> (Unchecked.defaultof<_>) @> with
        | Call (_, methodInfo, _) -> methodInfo.GetGenericMethodDefinition ()
        | _ -> invalidOp ""

    let bindMethodDefinition =
        match <@ this.Bind<obj, obj> (Unchecked.defaultof<_>, Unchecked.defaultof<_>) @> with
        | Call (_, methodInfo, _) -> methodInfo.GetGenericMethodDefinition ()
        | _ -> invalidOp ""

    // Right now, Bind and Return are just marker methods. It would be nice if they could be used directly in some
    // respect, but they need to operate on Exprs, which implies needing [<ReflectedDefinition>]... but that
    // automatically quotes the arguments in the overall AST, which not only limits what can make it intot he AST,
    // but adds extra overhead and accidental complexity.
    member this.Bind<'T, 'U> (argument : IDivinable<'T>, body : 'T -> IDivinable<'U>) : IDivinable<'U> =
        // Extract arguments of right-hand side into identities
        // Insert identities of arguments into scope to be used when identifying the divinable from right-hand side
        // Identify the divinable from right-hand side
        // Insert that identity into scope for the body's divinable, as the var representing the argument to body (aka the right-hand side)
        // 
        invalidOp "" : IDivinable<'U>

    member this.BindExpr<'T, 'U> (argumentExpr : Expr(*<IDivinable<'T>>*), bodyExpr : Expr(*<'T -> IDivinable<'U>>*)) : IDivinable<'U> =
        let argumentDivinable = this.DivinableExprToScopedDivinable<'T> (argumentExpr)
        Divinable<'U> (fun (scope, diviner) ->
            let argumentDivinableType = argumentDivinable.GetType ()
            let argumentDivinableInterfaceType = argumentDivinableType.GetInterfaces () |> Array.find (fun i -> i.Name.StartsWith "IDivinable")
            let argumentIdentifyMethod = argumentDivinableInterfaceType.GetMethod ("Identify")
            let argumentIdentifyReturnValue = argumentIdentifyMethod.Invoke (argumentDivinable, [|scope; diviner|])
            let argumentIdentity = argumentIdentifyReturnValue :?> Identity
            match bodyExpr with
            | Lambda (_, Let (letVar, _, letBody)) ->
                let bodyIdentity = letBody.ToIdentity ()
                bodyIdentity
            | _ -> invalidOp ""
        ) :> _

    member this.BindExprWrapper (argumentExpr : Expr, bodyExpr : Expr, argumentType : Type, returnType : Type) =
        match <@ this.BindExpr<obj, obj> (obj () :?> _, obj () :?> _) @> with
        | Call (_, objMethodInfo, _) ->
            let methodDefinition = objMethodInfo.GetGenericMethodDefinition ()
            let targetMethodInfo = methodDefinition.MakeGenericMethod [|argumentType; returnType|]
            targetMethodInfo.Invoke (this, [|argumentExpr; bodyExpr|])
        | _ -> invalidOp ""

    member this.DivinableExprToScopedDivinableWrapper (divinableExpr : Expr, divinableType : Type) =
        match <@ this.DivinableExprToScopedDivinable<obj> (obj () :?> _) @> with
        | Call (_, objMethodInfo, _) ->
            let methodDefinition = objMethodInfo.GetGenericMethodDefinition ()
            let targetMethodInfo = methodDefinition.MakeGenericMethod [|divinableType|]
            targetMethodInfo.Invoke (this, [|divinableExpr|])
        | _ -> invalidOp ""

    member this.DivinableExprToScopedDivinable<'T> (divinableExpr : Expr) : IDivinable<'T> =
        match divinableExpr with
        | Call (this', methodInfo, arguments) ->
            let this'' =
                match this' with
                | Some t -> Some (t.ToIdentity ())
                | None -> None
            let arguments' = List.map (fun (a : Expr) -> a.ToIdentity ()) arguments
            let parameterInfos = methodInfo.GetParameters ()
            Divinable<'T> (fun (scope, diviner) ->
                let scopeWithArguments =
                    Seq.zip arguments' parameterInfos
                    |> Seq.fold (fun (s : IdentificationScope) (a, p) ->
                        IdentificationScope.add (VarIdentity (p.Name, p.ParameterType)) a s
                    ) scope
                let scopeWithThis =
                    match this'' with
                    | Some t -> IdentificationScope.add (VarIdentity ("this", this'.Value.GetType ())) t scopeWithArguments
                    | None -> scopeWithArguments

                let this''' =
                    match this'' with
                    | Some t -> diviner.ResolveValue (scope, t)
                    | None -> null
                let arguments'' = List.map (fun a -> diviner.ResolveValue (scope, a)) arguments' |> List.toArray

                let divinable : IDivinable<'T> = methodInfo.Invoke (this''', arguments'') :?> _

                divinable.Identify (scopeWithThis, diviner)
            ) :> _
        | _ ->
            invalidOp ""

    member this.Return<'T> (_ : 'T) : IDivinable<'T> =
        invalidOp "" : IDivinable<'T>

    member this.Run ([<ReflectedDefinition>] runExpr : Expr<IDivinable<'T>>) : IDivinable<'T> =
        let runExprString = runExpr.ToString ()
        match runExpr with
        | Call (Some (Value (this', _)), methodInfo, argumentExprs) ->
            if methodInfo.IsGenericMethod then
                let methodDefinition = methodInfo.GetGenericMethodDefinition ()
                if methodDefinition = returnMethodDefinition then
                    match argumentExprs with
                    | [returnArgumentExpr] ->
                        let returnArgumentExprString = returnArgumentExpr.ToString ()
                        let returnArgumentIdentity = returnArgumentExpr.ToIdentity ()
                        Divinable<_> (fun _ -> returnArgumentIdentity) :> _
                    | _ -> invalidOp ""
                else if methodDefinition = bindMethodDefinition then
                    match argumentExprs with
                    | [bindArgumentExpr; bindBodyExpr] ->
                        let bindArgumentExprString = bindArgumentExpr.ToString ()
                        match methodInfo.GetGenericArguments () with
                        | [|bindArgumentType; bindReturnType|] ->
                            this.BindExprWrapper (bindArgumentExpr, bindBodyExpr, bindArgumentType, bindReturnType) :?> _
                            //let bindArgument = this.DivinableExprToScopedDivinableWrapper (bindArgumentExpr, bindArgumentType)
                            //bindArgument :?> _
                            //let bindBodyExprString = bindBodyExpr.ToString ()
                            //let bindArgument = QuotationEvaluator.EvaluateUntyped bindArgumentExpr
                            //let bindBody = QuotationEvaluator.EvaluateUntyped bindBodyExpr
                            //methodInfo.Invoke (this, [|bindArgumentExpr; bindBodyExpr|]) :?> IDivinable<'T>
                            //let bindArgumentIdentity = bindArgumentExpr.ToIdentity ()
                            //Divinable<_> (fun _ -> bindArgumentIdentity) :> _
                        | _ -> invalidOp ""
                    | _ -> invalidOp ""
                else
                    invalidOp ""
            else
                invalidOp (sprintf "Wrong methodInfo: %A" methodInfo)
        | Let (var, argumentExpr, bodyExpr) ->
            let bodyIdentity =
                match bodyExpr with
                | Call (Some (Value (this, _)), methodInfo, argumentExprs) ->
                    if methodInfo.IsGenericMethod then
                        let methodDefinition = methodInfo.GetGenericMethodDefinition ()
                        if methodDefinition = returnMethodDefinition then
                            match argumentExprs with
                            | [returnArgumentExpr] ->
                                //let returnArgumentExprString = returnArgumentExpr.ToString ()
                                let returnArgumentIdentity = returnArgumentExpr.ToIdentity ()
                                returnArgumentIdentity
                                //Divinable<_> (fun _ -> returnArgumentIdentity) :> _
                            | _ -> invalidOp ""
                        else if methodDefinition = bindMethodDefinition then
                            match argumentExprs with
                            | [bindArgumentExpr; bindBodyExpr] ->
                                //let bindArgumentExprString = bindArgumentExpr.ToString ()
                                //let bindBodyExprString = bindBodyExpr.ToString ()
                                let bindArgumentIdentity = bindArgumentExpr.ToIdentity ()
                                bindArgumentIdentity
                                //Divinable<_> (fun _ -> bindArgumentIdentity) :> _
                            | _ -> invalidOp ""
                        else
                            invalidOp ""
                    else
                        invalidOp (sprintf "Wrong methodInfo: %A" methodInfo)
                | _ -> invalidOp ""
            let bodyExprString = bodyExpr.ToString ()
            let identity = LetIdentity (VarIdentity (var.Name, var.Type), argumentExpr.ToIdentity (), bodyIdentity)
            let identityString = identity.ToString ()
            let x = 5
            Divinable<_> (fun _ -> identity) :> _
        | _ -> invalidOp ""

    member this.ScopeFromExpr (expr : Expr) : IdentificationScope =
        let exprString = expr.ToString ()
        let myArgumentIdentity = VarIdentity ("myArgument", typeof<int>)
        IdentificationScope.empty ()
        |> IdentificationScope.add myArgumentIdentity (ValueIdentity (42 :> obj, typeof<int>))

    member this.DivinableFromDivinableExprAndBodyAndBodyExpr (divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>, bodyExpr : Expr<'T -> IDivinable<'U>>) : IDivinable<'U> =
        Divinable<'U> (fun (scope, diviner) ->
            let divinableIdentity = divinableExpr.ToIdentity ()
            let divinable : IDivinable<'U> =
                match divinableIdentity with
                | CallIdentity (this', methodInfo, arguments) ->
                    let parameterInfos = methodInfo.GetParameters ()
                    let scopeWithArguments = Seq.zip arguments parameterInfos |> Seq.fold (fun (s : IdentificationScope) (a, p) -> IdentificationScope.add (VarIdentity (p.Name, p.ParameterType)) a s) scope
                    diviner.ResolveValue (scopeWithArguments, divinableIdentity)
                | _ ->
                    diviner.ResolveValue (scope, divinableIdentity)
            let divinedValue = diviner.ResolveValue (scope, divinable.Identify (scope, diviner))
            let bodyDivinable = body divinedValue
            let bodyExprString = bodyExpr.ToString ()
            let bodyScopeWithArguments = scope |> IdentificationScope.add (VarIdentity ("returnValue", typeof<'T>)) (ValueIdentity (divinedValue :> obj, typeof<'T>))
            let bodyIdentity = bodyDivinable.Identify (bodyScopeWithArguments, diviner)
            bodyIdentity
        ) :> IDivinable<'U>