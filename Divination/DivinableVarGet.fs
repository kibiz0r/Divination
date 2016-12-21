namespace Divination

open System

type DivinableVarGet (name : string) =
    interface IDivinable

    member this.Name = name
