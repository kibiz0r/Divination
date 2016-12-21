﻿namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.ExprShape
open FSharp.Quotations.Evaluator

type Exalter () =
    let iDivinableType = typeof<IDivinable>
    let iDivinableGenericType = typeof<IDivinable<obj>>.GetGenericTypeDefinition ()

    let divinableCastMethodInfo =
        match <@@ Divinable.cast @@> with
        | Lambda (_, Call (None, methodInfo, _)) -> methodInfo.GetGenericMethodDefinition ()
        | _ -> raise (Exception "whoops")
    let divinableCast (divinable : IDivinable) (type' : Type) : IDivinable =
        let typedMethod = divinableCastMethodInfo.MakeGenericMethod [|type'|]
        typedMethod.Invoke (None, [|divinable|]) :?> IDivinable

    interface IExalter with
        member this.Exalt<'T> (toExalt : Expr<'T>) : IDivinable<'T> =
            let rec exalt expr =
                match expr with
                | Let (var, value, body) -> Divinable.let' (Divinable.var (var.Name, var.Type), exalt value, exalt body)
                | Value (value) -> Divinable.value value
                | Var (var) -> Divinable.varGet (var.Name)
                | Call (this', methodInfo, arguments) ->
                    let exaltedMethodAttributes = methodInfo.GetCustomAttributes (typeof<ExaltedMethodAttribute>, true)
                    if exaltedMethodAttributes |> Array.isEmpty then
                        raise (Exception "not exalted")
                    else
                        let exaltedMethodName = (exaltedMethodAttributes.[0] :?> ExaltedMethodAttribute).ExaltedMethodName
                        let exaltedMethod = methodInfo.ReflectedType.GetMethod exaltedMethodName

                        if exaltedMethod.IsGenericMethodDefinition then
                            raise (InvalidOperationException "Exalted methods must not be generic")

                        let this'' : obj = null

                        let exaltedMethodParameters = exaltedMethod.GetParameters ()
                        let arguments' : obj [] =
                            Seq.zip exaltedMethodParameters arguments
                            |> Seq.map (fun (parameter, argument) ->
                                let parameterType = parameter.ParameterType
                                if not <| iDivinableType.IsAssignableFrom parameterType then
                                    raise (ArgumentException ("Exalted parameters must accept IDivinable or a specific IDivinable<'T>", parameter.Name))
                                let exalted = exalt argument
                                if parameterType.IsGenericType then
                                    divinableCast exalted parameterType.GenericTypeArguments.[0]
                                else
                                    exalted
                            ) |> Seq.cast |> Seq.toArray

                        exaltedMethod.Invoke (this'', arguments') :?> IDivinable
                | _ -> raise (Exception "whoops")
            exalt toExalt |> Divinable.cast

        //member this.Exalt<'T> (toExalt : Expr<'T>) : Expr<IDivinable<'T>> =
        //    let rec exalt e =
        //        match e with
        //        | Let (var, expr, body) ->
        //            let var' = Var (var.Name, divinableType var.Type)
        //            let expr' = exalt expr
        //            let body' =
        //                match body with
        //                | Var (v) when v = var ->
        //                    Expr.Var var'
        //                | _ -> body
        //            Expr.Let (var', expr', body')
        //        | Value (v, t) ->
        //            let methodInfo = typeof<Divinable>.GetMethod "Value"
        //            let valueMethod = methodInfo.MakeGenericMethod t
        //            Expr.Call (valueMethod, [e])
        //        | _ -> <@@ Divinable.Value<'T> %%e @@>
        //    exalt toExalt |> Expr.Cast<IDivinable<'T>>