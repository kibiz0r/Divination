namespace Divination

open System
open FSharp.Interop.Dynamic

type Divinable<'T> (raw : IDivinable) =
    interface IDivinable<'T>

    member this.Raw = raw

type DivinableValueUnionCaseType = {
    Value : obj
}

type DivinableUnion =
    | DivinableValueUnionCase of DivinableValueUnionCaseType

module Divinable =
    let divine (diviner : IDiviner) (divinable : IDivinable) : Divined<'T> =
        let divinerType = diviner.GetType ()
        let value = diviner?Divine divinable

        {
            Source = divinable
            Value = value
        }

    let cast (divinable : IDivinable) : IDivinable<'T> =
        Divinable<'T> divinable :> IDivinable<'T>

    let value (value : obj, type' : Type) : IDivinable =
        DivinableValue (value, type') :> IDivinable

    let var (name : string, type' : Type) : DivineVar =
        { Name = name; Type = type' }

    let varGet (name : string) : IDivinable =
        DivinableVarGet name :> IDivinable

    let let' (var : DivineVar, value : IDivinable, body : IDivinable) : IDivinable =
        DivinableLet (var, value, body) :> IDivinable

    //static member Value (value : obj) =
    //    DivinableValue (value) :> IDivinable

    //static member Value<'T> (value : 'T) =
    //    DivinableValue (value) :> IDivinable<'T>