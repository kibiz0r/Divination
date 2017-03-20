namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

#nowarn "40"

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

    //member this.BindExpr<'T, 'U> (argumentExpr : Expr(*<IDivinable<'T>>*), bodyExpr : Expr(*<'T -> IDivinable<'U>>*)) : IDivinable<'U> =
    //    let argumentDivinable = this.DivinableExprToScopedDivinable<'T> (argumentExpr)
    //    Divinable<'U> (fun (scope, diviner) ->
    //        let argumentDivinableType = argumentDivinable.GetType ()
    //        let argumentDivinableInterfaceType = argumentDivinableType.GetInterfaces () |> Array.find (fun i -> i.Name.StartsWith "IDivinable")
    //        let argumentIdentifyMethod = argumentDivinableInterfaceType.GetMethod ("Identify")
    //        let argumentIdentifyReturnValue = argumentIdentifyMethod.Invoke (argumentDivinable, [|scope; diviner|])
    //        let argumentIdentity = argumentIdentifyReturnValue :?> Identity
    //        match bodyExpr with
    //        | Lambda (_, Let (letVar, _, letBody)) ->
    //            let bodyIdentity = letBody.ToIdentity ()
    //            bodyIdentity
    //        | _ -> invalidOp ""
    //    ) :> _

    //member this.BindExprWrapper (argumentExpr : Expr, bodyExpr : Expr, argumentType : Type, returnType : Type) =
    //    match <@ this.BindExpr<obj, obj> (obj () :?> _, obj () :?> _) @> with
    //    | Call (_, objMethodInfo, _) ->
    //        let methodDefinition = objMethodInfo.GetGenericMethodDefinition ()
    //        let targetMethodInfo = methodDefinition.MakeGenericMethod [|argumentType; returnType|]
    //        targetMethodInfo.Invoke (this, [|argumentExpr; bodyExpr|])
    //    | _ -> invalidOp ""

    //member this.IdentifyBindArgumentExpr (scope : IdentificationScope, diviner : IDiviner, argumentExpr : Expr, argumentType : Type) : Identity =
    //    let argumentDivinable = this.DivinableExprToScopedDivinableWrapper (argumentExpr, argumentType)
    //    let argumentDivinableType = argumentExpr.Type
    //    //let argumentDivinableInterfaceType = argumentDivinableType.GetInterfaces () |> Array.find (fun i -> i.Name.StartsWith "IDivinable")
    //    let argumentIdentifyMethod = argumentDivinableType.GetMethod ("Identify")
    //    let argumentIdentifyReturnValue = argumentIdentifyMethod.Invoke (argumentDivinable, [|scope; diviner|])
    //    let argumentIdentity = argumentIdentifyReturnValue :?> Identity
    //    argumentIdentity

    //member this.DivinableExprToScopedDivinableWrapper (divinableExpr : Expr, divinableType : Type) =
    //    match <@ this.DivinableExprToScopedDivinable<obj> (obj () :?> _) @> with
    //    | Call (_, objMethodInfo, _) ->
    //        let methodDefinition = objMethodInfo.GetGenericMethodDefinition ()
    //        let targetMethodInfo = methodDefinition.MakeGenericMethod [|divinableType|]
    //        targetMethodInfo.Invoke (this, [|divinableExpr|])
    //    | _ -> invalidOp ""

    //member this.DivinableExprToScopedDivinable<'T> (divinableExpr : Expr) : IDivinable<'T> =
    //    match divinableExpr with
    //    | Call (this', methodInfo, arguments) ->
    //        let this'' =
    //            match this' with
    //            | Some t -> Some (t.ToIdentity ())
    //            | None -> None
    //        let arguments' = List.map (fun (a : Expr) -> a.ToIdentity ()) arguments
    //        let parameterInfos = methodInfo.GetParameters ()
    //        Divinable<'T> (fun (scope, diviner) ->
    //            let scopeWithArguments =
    //                Seq.zip arguments' parameterInfos
    //                |> Seq.fold (fun (s : IdentificationScope) (a, p) ->
    //                    IdentificationScope.add (VarIdentity (p.Name)) a s
    //                ) scope
    //            let scopeWithThis =
    //                match this'' with
    //                | Some t -> IdentificationScope.add (VarIdentity "this") t scopeWithArguments
    //                | None -> scopeWithArguments

    //            let this''' =
    //                match this'' with
    //                | Some t -> diviner.ResolveValue (scope, t)
    //                | None -> null
    //            let arguments'' = List.map (fun a -> diviner.ResolveValue (scope, a)) arguments' |> List.toArray

    //            let r = methodInfo.Invoke (this''', arguments'')
    //            let divinable : IDivinable<'T> = r :?> _

    //            divinable.Identify (scopeWithThis, diviner)
    //        ) :> _
    //    | _ ->
    //        invalidOp ""

    member this.Return<'T> (_ : 'T) : IDivinable<'T> =
        invalidOp "" : IDivinable<'T>

    member this.Run ([<ReflectedDefinition>] runExpr : Expr<IDivinable<'T>>) : IDivinable<'T> =
        let runExprString = runExpr.ToString ()
        let rec exprDivinifier : IExprDivinifier =
            ExprDivinifier (
                fun (expr : Expr) ->
                    match expr with
                    | Call (this', methodInfo, argumentExprs) ->
                        if this' = Some (Expr.Value this) && methodInfo.IsGenericMethod then
                            let methodDefinition = methodInfo.GetGenericMethodDefinition ()
                            if methodDefinition = returnMethodDefinition then
                                match argumentExprs with
                                | [returnArgumentExpr] ->
                                    Some (exprDivinifier.ToDivinableBase returnArgumentExpr)
                                | _ -> invalidOp ""
                            else if methodDefinition = bindMethodDefinition then
                                match argumentExprs with
                                | [bindArgumentExpr; bindBodyExpr] ->
                                    match bindBodyExpr with
                                    | Lambda (lambdaVar, Let (letVar, letArgument, letBody)) ->
                                        let bindArgument = Divinable (fun context ->
                                            context.Return (ValueIdentity ("bindeded" :> obj, typeof<string>))
                                            //let bindArgumentExprDivinable = exprDivinifier.ToDivinableBase bindArgumentExpr
                                            //let theContext = (scope, diviner)
                                            //let contextDoSomething (divinable : IDivinableBase) =
                                            //    diviner.ResolveValue<IDivinableBase> (scope, divinable.Identify theContext)
                                            //let bindArgumentDivinable = diviner.ResolveValue<IDivinableBase> (scope, bindArgumentExprDivinable.Identify (scope, diviner))
                                            //let bindArgumentIdentity = bindArgumentDivinable.Identify (scope, diviner)
                                            //bindArgumentIdentity
                                        )
                                        Some (bindArgument :> _)
                                        //Some (DivinableBase.let' (DivinableBase.var letVar.Name) bindArgument (exprDivinifier.ToDivinableBase letBody))
                                    | _ -> None
                                | _ -> None
                            else
                                None
                        else
                            None
                    | _ -> None
                ) :> _
        Divinable<_> (fun context ->
            context.Return (ValueIdentity ("idk what this is" :> obj, typeof<string>))
            //let runDivinable = exprDivinifier.ToDivinable runExpr
            //runDivinable.Identify (scope, diviner)
        ) :> _
        //match runExpr with
        //| Call (this', methodInfo, argumentExprs) ->
        //    if this' = Some (Expr.Value this) && methodInfo.IsGenericMethod then
        //        let methodDefinition = methodInfo.GetGenericMethodDefinition ()
        //        if methodDefinition = returnMethodDefinition then
        //            match argumentExprs with
        //            | [returnArgumentExpr] ->
        //                exprDivinifier.ToDivinable (Expr.Cast returnArgumentExpr)
        //            | _ -> invalidOp ""
        //        else if methodDefinition = bindMethodDefinition then
        //            match argumentExprs with
        //            | [bindArgumentExpr; bindLambdaExpr] ->
        //                match bindLambdaExpr with
        //                | Lambda (_, Let (bindVar, _, bindBodyExpr)) ->
        //                    Divinable<_> (fun (scope, diviner) ->
        //                        let var = exprDivinifier.ToDivinableBase bindVar
        //                        let argumentDivinable = exprDivinifier.ToDivinableBase bindArgumentExpr
        //                        let argumentDivinableIdentity = argumentDivinable.Identify (scope, diviner)
        //                        let argument = diviner.ResolveValue<IDivinableBase> (scope, argumentDivinableIdentity)
        //                        let body = exprDivinifier.ToDivinableBase bindBodyExpr
        //                        let let' = DivinableBase.let' var argument body
        //                        let i = let'.Identify (scope, diviner)
        //                        i
        //                    ) :> _
        //                | _ -> invalidOp ""
        //            | _ -> invalidOp ""
        //        else
        //            invalidOp ""
        //    else
        //        invalidOp ""
        //| _ -> invalidOp ""
        //let rec walkExpr (expr : Expr) : IdentificationScope -> IDiviner -> Identity =
        //    match expr with
        //    | Call (this', methodInfo, argumentExprs) ->
        //        let handleCallNormally =
        //            fun scope diviner ->
        //                let thisIdentity =
        //                    match this' with
        //                    | Some t -> Some (walkExpr t scope diviner)
        //                    | None -> None
        //                let argumentIdentities = List.map (fun a -> walkExpr a scope diviner) argumentExprs
        //                CallIdentity (thisIdentity, methodInfo, argumentIdentities)
        //        if this' = Some (Expr.Value this) && methodInfo.IsGenericMethod then
        //            let methodDefinition = methodInfo.GetGenericMethodDefinition ()
        //            if methodDefinition = returnMethodDefinition then
        //                match argumentExprs with
        //                | [returnArgumentExpr] ->
        //                    walkExpr returnArgumentExpr
        //                | _ -> invalidOp "Wrong Return method arguments"
        //            else if methodDefinition = bindMethodDefinition then
        //                match methodInfo.GetGenericArguments () with
        //                | [|bindArgumentType; bindReturnType|] ->
        //                    match argumentExprs with
        //                    | [bindArgumentExpr; bindBodyExpr] ->
        //                        let bindArgumentExprString = bindArgumentExpr.ToString ()
        //                        match bindBodyExpr with
        //                        | Lambda (_, Let (letVar, _, letBody)) ->
        //                            fun scope diviner ->
        //                                let bindArgumentIdentity = this.IdentifyBindArgumentExpr (scope, diviner, bindArgumentExpr, bindArgumentType)
        //                                let scope = IdentificationScope.add (VarIdentity letVar.Name) bindArgumentIdentity scope
        //                                walkExpr letBody scope diviner
        //                        | _ -> invalidOp "Wrong Bind method body Expr shape"
        //                    | _ -> invalidOp "Wrong Bind method arguments"
        //                | _ -> invalidOp "Wrong Bind method type arguments"
        //            else
        //                handleCallNormally
        //        else
        //            handleCallNormally
        //    | Var (var) ->
        //        fun scope diviner ->
        //            let varIdentity = VarIdentity var.Name
        //            match IdentificationScope.tryFind varIdentity scope with
        //            | Some bound -> bound
        //            | None -> invalidOp (sprintf "Couldn't find var: %A" varIdentity)
        //    | Let (letVar, letArgument, letBody) ->
        //        fun scope diviner ->
        //            let letArgumentIdentity = walkExpr letArgument scope diviner
        //            let scope = IdentificationScope.add (VarIdentity letVar.Name) letArgumentIdentity scope
        //            walkExpr letBody scope diviner
        //    | Value (value, type') ->
        //        fun scope diviner ->
        //            ValueIdentity (value, type')
        //    | NewObject (constructorInfo, argumentExprs) ->
        //        fun scope diviner ->
        //            let argumentIdentities = List.map (fun a -> walkExpr a scope diviner) argumentExprs
        //            ConstructorIdentity (constructorInfo, argumentIdentities)
        //    | _ -> invalidOp (sprintf "Expr type not handled yet: %A" expr)
        //Divinable<_> (fun (scope, diviner) ->
        //    walkExpr runExpr scope diviner
        //) :> _
        //match runExpr with
        //| Call (Some (Value (this', _)), methodInfo, argumentExprs) ->
        //    if methodInfo.IsGenericMethod then
        //        let methodDefinition = methodInfo.GetGenericMethodDefinition ()
        //        if methodDefinition = returnMethodDefinition then
        //            match argumentExprs with
        //            | [returnArgumentExpr] ->
        //                let returnArgumentExprString = returnArgumentExpr.ToString ()
        //                let returnArgumentIdentity = returnArgumentExpr.ToIdentity ()
        //                Divinable<_> (fun _ -> returnArgumentIdentity) :> _
        //            | _ -> invalidOp ""
        //        else if methodDefinition = bindMethodDefinition then
        //            match argumentExprs with
        //            | [bindArgumentExpr; bindBodyExpr] ->
        //                let bindArgumentExprString = bindArgumentExpr.ToString ()
        //                match methodInfo.GetGenericArguments () with
        //                | [|bindArgumentType; bindReturnType|] ->
        //                    this.BindExprWrapper (bindArgumentExpr, bindBodyExpr, bindArgumentType, bindReturnType) :?> _
        //                    //let bindArgument = this.DivinableExprToScopedDivinableWrapper (bindArgumentExpr, bindArgumentType)
        //                    //bindArgument :?> _
        //                    //let bindBodyExprString = bindBodyExpr.ToString ()
        //                    //let bindArgument = QuotationEvaluator.EvaluateUntyped bindArgumentExpr
        //                    //let bindBody = QuotationEvaluator.EvaluateUntyped bindBodyExpr
        //                    //methodInfo.Invoke (this, [|bindArgumentExpr; bindBodyExpr|]) :?> IDivinable<'T>
        //                    //let bindArgumentIdentity = bindArgumentExpr.ToIdentity ()
        //                    //Divinable<_> (fun _ -> bindArgumentIdentity) :> _
        //                | _ -> invalidOp ""
        //            | _ -> invalidOp ""
        //        else
        //            invalidOp ""
        //    else
        //        invalidOp (sprintf "Wrong methodInfo: %A" methodInfo)
        //| Let (var, argumentExpr, bodyExpr) ->
        //    let bodyIdentity =
        //        match bodyExpr with
        //        | Call (Some (Value (this, _)), methodInfo, argumentExprs) ->
        //            if methodInfo.IsGenericMethod then
        //                let methodDefinition = methodInfo.GetGenericMethodDefinition ()
        //                if methodDefinition = returnMethodDefinition then
        //                    match argumentExprs with
        //                    | [returnArgumentExpr] ->
        //                        //let returnArgumentExprString = returnArgumentExpr.ToString ()
        //                        let returnArgumentIdentity = returnArgumentExpr.ToIdentity ()
        //                        returnArgumentIdentity
        //                        //Divinable<_> (fun _ -> returnArgumentIdentity) :> _
        //                    | _ -> invalidOp ""
        //                else if methodDefinition = bindMethodDefinition then
        //                    match argumentExprs with
        //                    | [bindArgumentExpr; bindBodyExpr] ->
        //                        //let bindArgumentExprString = bindArgumentExpr.ToString ()
        //                        //let bindBodyExprString = bindBodyExpr.ToString ()
        //                        let bindArgumentIdentity = bindArgumentExpr.ToIdentity ()
        //                        bindArgumentIdentity
        //                        //Divinable<_> (fun _ -> bindArgumentIdentity) :> _
        //                    | _ -> invalidOp ""
        //                else
        //                    invalidOp ""
        //            else
        //                invalidOp (sprintf "Wrong methodInfo: %A" methodInfo)
        //        | _ -> invalidOp ""
        //    let bodyExprString = bodyExpr.ToString ()
        //    let identity = LetIdentity (VarIdentity (var.Name, var.Type), argumentExpr.ToIdentity (), bodyIdentity)
        //    let identityString = identity.ToString ()
        //    let x = 5
        //    Divinable<_> (fun _ -> identity) :> _
        //| _ -> invalidOp ""

    //member this.ScopeFromExpr (expr : Expr) : IdentificationScope =
    //    let exprString = expr.ToString ()
    //    let myArgumentIdentity = VarIdentity "myArgument"
    //    IdentificationScope.empty ()
    //    |> IdentificationScope.add myArgumentIdentity (ValueIdentity (42 :> obj, typeof<int>))

    //member this.DivinableFromDivinableExprAndBodyAndBodyExpr (divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>, bodyExpr : Expr<'T -> IDivinable<'U>>) : IDivinable<'U> =
    //    Divinable<'U> (fun (scope, diviner) ->
    //        let divinableIdentity = divinableExpr.ToIdentity ()
    //        let divinable : IDivinable<'U> =
    //            match divinableIdentity with
    //            | CallIdentity (this', methodInfo, arguments) ->
    //                let parameterInfos = methodInfo.GetParameters ()
    //                let scopeWithArguments = Seq.zip arguments parameterInfos |> Seq.fold (fun (s : IdentificationScope) (a, p) -> IdentificationScope.add (VarIdentity p.Name) a s) scope
    //                diviner.ResolveValue (scopeWithArguments, divinableIdentity)
    //            | _ ->
    //                diviner.ResolveValue (scope, divinableIdentity)
    //        let divinedValue = diviner.ResolveValue (scope, divinable.Identify (scope, diviner))
    //        let bodyDivinable = body divinedValue
    //        let bodyExprString = bodyExpr.ToString ()
    //        let bodyScopeWithArguments = scope |> IdentificationScope.add (VarIdentity "returnValue") (ValueIdentity (divinedValue :> obj, typeof<'T>))
    //        let bodyIdentity = bodyDivinable.Identify (bodyScopeWithArguments, diviner)
    //        bodyIdentity
    //    ) :> IDivinable<'U>