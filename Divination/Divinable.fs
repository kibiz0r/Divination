namespace Divination

open System
open FSharp.Interop.Dynamic

type Divinable =
    | DivinableValue of DivinableValue
    | DivinableLet of DivinableLet
    | DivinableVarGet of DivinableVarGet

and DivinableLet = {
    Var : DivineVar
    Value : Divinable
    Body : Divinable
}

and DivinableValue = {
    Value : obj
    Type : Type
}

and DivinableVarGet = {
    Name : string
}

and IDiviner =
    abstract member Let : DivinableLet -> obj
    abstract member Value : DivinableValue -> obj
    abstract member VarGet : DivinableVarGet -> obj

type Divinable<'T> = {
    Raw : Divinable
}

type Divined = {
    Source : Divinable
    Value : obj
}

type Divined<'T> = {
    Source : Divinable<'T>
    Value : 'T
}

module Divinable =
    let divine (diviner : IDiviner) (divinable : Divinable) : Divined =
        let divinerType = diviner.GetType ()
        let value =
            match divinable with
            | Divinable.DivinableLet let' -> diviner.Let let'
            | Divinable.DivinableValue value -> diviner.Value value
            | Divinable.DivinableVarGet varGet -> diviner.VarGet varGet

        {
            Source = divinable
            Value = value :?> 'T
        }

    let cast (divinable : Divinable) : Divinable<'T> =
        { Raw = divinable }

    let value (value : obj, type' : Type) : Divinable =
        DivinableValue { Value = value; Type = type' }

    let var (name : string, type' : Type) : DivineVar =
        { Name = name; Type = type' }

    let varGet (name : string) : Divinable =
        DivinableVarGet { Name = name }

    let let' (var : DivineVar, value : Divinable, body : Divinable) : Divinable =
        DivinableLet { Var = var; Value = value; Body = body }

    //static member Value (value : obj) =
    //    DivinableValue (value) :> IDivinable

    //static member Value<'T> (value : 'T) =
    //    DivinableValue (value) :> IDivinable<'T>