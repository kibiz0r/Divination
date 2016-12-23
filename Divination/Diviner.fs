namespace Divination

open System
open MethodInfoExtensions

type Diviner () as this =
    let eval expr context =
        (this :> IDiviner<'Context>).Eval (expr, context)

    interface IDiviner<IDiviningContext> with
        member this.Eval (divinedExpr, context) =
            match divinedExpr with
            | DivinedExpr.DivinedValue { Value = value; TypeName = typeName } ->
                let type' = Type.GetType typeName
                if value.GetType () = type' then
                    value
                else
                    Convert.ChangeType (value, type')
            | DivinedExpr.DivinedLet { Var = var; Value = value; Body = body } ->
                let value' = eval value context
                let newContext = context.SetVar (var.Name, value')
                eval body newContext
            | DivinedExpr.DivinedCall { This = this'; TypeName = typeName; MethodName = methodName; Arguments = arguments } ->
                let this'' =
                    match this' with
                    | Some t -> eval t context
                    | None -> null
                let type' = Type.GetType typeName
                let methodInfo = type'.GetMethod methodName
                let arguments' = arguments |> List.map (fun a -> eval a context) |> List.toArray
                methodInfo.InvokeGeneric<string> (this'', arguments')
            | DivinedExpr.DivinedVarGet { Var = var } ->
                context.GetVar var.Name
            | _ ->
                raise (Exception (sprintf "Unknown expression type: %A" divinedExpr))

    //member this.Divine (value : DivinableValueUnionCaseType) : obj =
    //    raise (Exception "got union case type")