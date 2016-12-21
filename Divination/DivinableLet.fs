namespace Divination

open System

type DivinableLet (var : DivineVar, value : IDivinable, body : IDivinable) =
    interface IDivinable

    member this.Var = var
    member this.Value = value
    member this.Body = body
