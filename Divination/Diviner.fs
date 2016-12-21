namespace Divination

open System

type Diviner () =
    interface IDiviner with
        member this.Eval divinedExpr =
            match divinedExpr with
            | DivinedExpr.DivinedValue { Value = value } -> value
            | _ -> obj ()

    //member this.Divine (value : DivinableValueUnionCaseType) : obj =
    //    raise (Exception "got union case type")