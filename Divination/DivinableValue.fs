namespace Divination

open System

type DivinableValue (value : obj, type' : Type) =
    interface IDivinable

    member this.Value = value
    member this.Type = type'