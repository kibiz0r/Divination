namespace Divination.FSharp

open System
open Divination
open MethodInfoExtensions

type FSharpDiviner () as this =
    let eval =
        Diviner.eval this

    interface IFSharpDiviner with
        member this.NewContext () =
            FSharpDiviningContext () :> IFSharpDiviningContext

        member this.Eval (divineExpr, context) =
            match divineExpr with
            | FSharpExpr.FSharpCall { This = this'; MethodInfo = methodInfo; Arguments = arguments } ->
                let this'' =
                    match this' with
                    | Some t -> Some (eval t context)
                    | None -> None
                let arguments' = arguments |> List.map (fun a -> eval a context) |> List.toArray
                methodInfo.Invoke (this'', arguments')
            | _ ->
                raise (Exception (sprintf "Unknown expression type: %A" divineExpr))
            //| DivinedExpr.DivinedValue { Value = value; TypeName = typeName } ->
            //    let type' = Type.GetType typeName
            //    if value.GetType () = type' then
            //        value
            //    else
            //        Convert.ChangeType (value, type')
            //| DivinedExpr.DivinedLet { Var = var; Value = value; Body = body } ->
            //    let value' = eval value context
            //    let newContext = context.SetVar (var.Name, value')
            //    eval body newContext
            //| DivinedExpr.DivinedCall { This = this'; TypeName = typeName; MethodName = methodName; Arguments = arguments } ->
            //    let this'' =
            //        match this' with
            //        | Some t -> eval t context
            //        | None -> null
            //    let type' = Type.GetType typeName
            //    let methodInfo = type'.GetMethod methodName
            //    let arguments' = arguments |> List.map (fun a -> eval a context) |> List.toArray
            //    methodInfo.InvokeGeneric<string> (this'', arguments')
            //| DivinedExpr.DivinedVarGet { Var = var } ->
            //    context.GetVar var.Name
