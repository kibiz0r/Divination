namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.ExprShape
open FSharp.Quotations.Evaluator

module Exalter =
    let rec traverse (exalter : IExalter<_, _, _, _, _, 'Expr>) (toTraverse : Expr) : 'Expr =
        let traverse = traverse exalter
        match toTraverse with
        | Call (this', methodInfo, arguments) ->
            let this'' =
                match this' with
                | Some t -> Some (traverse t)
                | None -> None
            let arguments' = arguments |> List.map (fun a -> traverse a)
            exalter.Call (this'', exalter.MethodInfo methodInfo, arguments')
        | Let (var, value, body) ->
            exalter.Let (exalter.Var var, traverse value,  traverse body)
        | Value (value, type') ->
            let type'' = exalter.Type type'
            exalter.Value (exalter.Primitive (value, type''), type'')
        | Var (var) ->
            exalter.VarGet (exalter.Var var)
        | _ ->
            raise (Exception (sprintf "Unrecognized expression %A" toTraverse))

    let exalt (exalter : IExalter<'Exalted, 'Primitive, 'Type, 'MethodInfo, 'Var, 'Expr>) (toExalt : Expr) : 'Exalted =
        exalter.Exalt (traverse exalter toExalt)

//type Exalter () =
//    let iDivinableType = typeof<IDivinable>
//    let iDivinableGenericType = typeof<IDivinable<obj>>.GetGenericTypeDefinition ()

//    let divinableCastMethodInfo =
//        match <@@ Divinable.cast @@> with
//        | Lambda (_, Call (None, methodInfo, _)) -> methodInfo.GetGenericMethodDefinition ()
//        | _ -> raise (Exception "whoops")
//    let divinableCast (divinable : IDivinable) (type' : Type) : IDivinable =
//        let typedMethod = divinableCastMethodInfo.MakeGenericMethod [|type'|]
//        typedMethod.Invoke (None, [|divinable|]) :?> IDivinable

//    interface IExalter with
//        member this.Exalt<'T> (toExalt : Expr<'T>) : IDivinable<'T> =
//            let rec exalt expr =
//                match expr with
//                | Let (var, value, body) -> Divinable.let' (Divinable.var (var.Name, var.Type.AssemblyQualifiedName), exalt value, exalt body)
//                | Value (value, type') -> Divinable.value (value, type'.AssemblyQualifiedName)
//                | Var (var) -> Divinable.varGet { Name = var.Name; TypeName = var.Type.AssemblyQualifiedName }
//                | Call (this', methodInfo, arguments) ->
//                    let exaltedMethodAttributes = methodInfo.GetCustomAttributes (typeof<ExaltedMethodAttribute>, true)
//                    if exaltedMethodAttributes |> Array.isEmpty then
//                        let this'' =
//                            match this' with
//                            | Some t -> Some (exalt t)
//                            | None -> None
//                        let arguments' = arguments |> List.map (fun a -> exalt a)
//                        Divinable.call (this'', methodInfo.DeclaringType.AssemblyQualifiedName, methodInfo.Name, arguments')
//                    else
//                        let exaltedMethodName = (exaltedMethodAttributes.[0] :?> ExaltedMethodAttribute).ExaltedMethodName
//                        let exaltedMethod = methodInfo.ReflectedType.GetMethod exaltedMethodName

//                        if exaltedMethod.IsGenericMethodDefinition then
//                            raise (InvalidOperationException "Exalted methods must not be generic")

//                        let this'' : obj = null

//                        let exaltedMethodParameters = exaltedMethod.GetParameters ()
//                        let arguments' : obj [] =
//                            Seq.zip exaltedMethodParameters arguments
//                            |> Seq.map (fun (parameter, argument) ->
//                                let parameterType = parameter.ParameterType
//                                if not <| iDivinableType.IsAssignableFrom parameterType then
//                                    raise (ArgumentException ("Exalted parameters must accept IDivinable or a specific IDivinable<'T>", parameter.Name))
//                                let exalted = exalt argument
//                                if parameterType.IsGenericType then
//                                    divinableCast exalted parameterType.GenericTypeArguments.[0]
//                                else
//                                    exalted
//                            ) |> Seq.cast |> Seq.toArray

//                        exaltedMethod.Invoke (this'', arguments') :?> IDivinable
//                | _ -> raise (Exception (sprintf "Unrecognized expression %A" expr))
//            exalt toExalt |> Divinable.cast

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