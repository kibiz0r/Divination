namespace Divination.FSharp

open System
open System.Reflection
open Divination

type IFSharpExalter =
    inherit IExalter<FSharpDivinable, obj, Type, MethodInfo, FSharpVar, FSharpExpr>

type FSharpExalter () =
    interface IFSharpExalter with
        member this.Exalt (expr : FSharpExpr) : FSharpDivinable =
            FSharpDivinable.fromExpr expr

        member this.Primitive (value, type') = value
        member this.Type (type') = type'
        member this.MethodInfo (methodInfo) = methodInfo
        member this.Var (var) = { Var = var }

        member this.Call (this', methodInfo, arguments) =
            let this'' =
                match this' with
                | Some t -> Some (t :> IDivineExpr)
                | None -> None
            let arguments' = arguments |> List.map (fun a -> a :> IDivineExpr)
            FSharpCall { This = this''; MethodInfo = methodInfo; Arguments = arguments' }

        member this.Let (var, value, body) =
            FSharpLet { Var = var; Value = value; Body = body }

        member this.Value (value, type') =
            FSharpValue { Value = value; Type = type' }

        member this.VarGet (var) =
            FSharpVarGet { Var = var }