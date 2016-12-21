namespace Divination

open System

type Diviner () =
    interface IDiviner with
        member this.Let (let' : DivinableLet) : obj =
            obj ()

        member this.Value (value : DivinableValue) : obj =
            value.Value

        member this.VarGet (varGet : DivinableVarGet) : obj =
            obj ()

    //member this.Divine (value : DivinableValueUnionCaseType) : obj =
    //    raise (Exception "got union case type")