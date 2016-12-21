namespace Divination

open System
open FSharp.Interop.Dynamic

// F# Expr: has more power than we can hope to translate, and also takes things too literally
// |> Divinable: limited to the only what we can express when fully materialized into the runtime, but speaks in terms of Exprs and IDivinables and incorporates hooks that allow custom expression nodes
// |> IDivinable: totally flexible, but must produce a DivinedExpr when given a Diviner (or maybe something related, like a DivineContext?), which that Diviner will then evaluate
// |> DivinedExpr: the complete, serializable (primitive data only) expression tree, which represents the actual path of execution to generate a Divined value; the same DivinedExpr must always produce the same Divined value

type DivinedVar = {
    Name : string
    Type : Type
}

type DivinedExpr =
    | DivinedCall of DivinedCallExpr
    | DivinedDivinerCall of DivinedDivinerCallExpr
    | DivinedLet of DivinedLetExpr
    | DivinedValue of DivinedValueExpr
    | DivinedVarGet of DivinedVarGetExpr

and DivinedCallExpr = {
    This : DivinedExpr option
    MethodName : string
    Arguments : DivinedExpr list
}

and DivinedDivinerCallExpr = {
    MethodName : string
    Arguments : DivinedExpr list
}

and DivinedLetExpr = {
    Var : DivinedVar
    Value : DivinedExpr
    Body : DivinedExpr
}

and DivinedValueExpr = {
    Value : obj
    Type : Type
}

and DivinedVarGetExpr = {
    Name : string
}

type IDiviner =
    //abstract member Let : DivinableLet -> obj
    //abstract member Value : DivinableValue -> obj
    //abstract member VarGet : DivinableVarGet -> obj
    abstract member Eval : DivinedExpr -> obj

type IDivinable =
    abstract member DivineExpr : IDiviner -> DivinedExpr

type IDivinable<'T> =
    inherit IDivinable
    abstract member Raw : IDivinable

type DivinableVar = {
    Name : string
    Type : Type
}

type Divinable<'T> (raw : IDivinable) =
    member this.Raw = raw

    override this.GetHashCode() =
        hash raw

    override this.Equals(other) =
        match other with
        | :? Divinable<'T> as o -> raw = o.Raw
        | _ -> false

    interface IDivinable<'T> with
        member this.Raw = raw

    interface IDivinable with
        member this.DivineExpr diviner = raw.DivineExpr diviner

type DivinableCall (this' : IDivinable option, methodName : string, arguments : IDivinable list) =
    member this.This = this'
    member this.MethodName = methodName
    member this.Arguments = arguments

    override this.GetHashCode() =
        hash (this', methodName, arguments)

    override this.Equals(other) =
        match other with
        | :? DivinableCall as o -> (this', methodName, arguments) = (o.This, o.MethodName, o.Arguments)
        | _ -> false

    interface IDivinable with
        member this.DivineExpr diviner =
            let this'' =
                match this' with
                | Some t -> Some (t.DivineExpr diviner)
                | None -> None
            let arguments' = arguments |> List.map (fun a -> a.DivineExpr diviner)
            DivinedExpr.DivinedCall { This = this''; MethodName = methodName; Arguments = arguments' }

type DivinableLet (var : DivinableVar, value : IDivinable, body : IDivinable) =
    member this.Var = var
    member this.Value = value
    member this.Body = body

    override this.GetHashCode() =
        hash (var, value, body)

    override this.Equals(other) =
        match other with
        | :? DivinableLet as o -> (var, value, body) = (o.Var, o.Value, o.Body)
        | _ -> false

    interface IDivinable with
        member this.DivineExpr diviner =
            let var' : DivinedVar = { Name = var.Name; Type = var.Type }
            let value' = value.DivineExpr diviner
            let body' = value.DivineExpr diviner
            DivinedExpr.DivinedLet { Var = var'; Value = value'; Body = body' }

type DivinableValue (value : obj, type' : Type) =
    member this.Value = value
    member this.Type = type'

    override this.GetHashCode() =
        hash (value, type')

    override this.Equals(other) =
        match other with
        | :? DivinableValue as o -> (value, type') = (o.Value, o.Type)
        | _ -> false

    interface IDivinable with
        member this.DivineExpr diviner =
            DivinedExpr.DivinedValue { Value = value; Type = type' }

type DivinableVarGet (name : string) =
    member this.Name = name

    override this.GetHashCode() =
        hash name

    override this.Equals(other) =
        match other with
        | :? DivinableVarGet as o -> name = o.Name
        | _ -> false

    interface IDivinable with
        member this.DivineExpr diviner =
            DivinedExpr.DivinedVarGet { Name = name }

type Divined<'T> = {
    Source : DivinedExpr
    Value : 'T
}

module Divinable =
    let divine (diviner : IDiviner) (divinable : IDivinable<'T>) : Divined<'T> =
        let divinedExpr = divinable.DivineExpr diviner
        let value = diviner.Eval divinedExpr

        {
            Source = divinedExpr
            Value = value :?> 'T
        }

    let cast (divinable : IDivinable) : IDivinable<'T> =
        Divinable<'T> divinable :> IDivinable<'T>

    let call (this : IDivinable option, methodName : string, arguments : IDivinable list) : IDivinable =
        DivinableCall (this, methodName, arguments) :> IDivinable

    let let' (var : DivinableVar, value : IDivinable, body : IDivinable) : IDivinable =
        DivinableLet (var, value, body) :> IDivinable

    let value (value : obj, type' : Type) : IDivinable =
        DivinableValue (value, type') :> IDivinable

    let var (name : string, type' : Type) : DivinableVar =
        { Name = name; Type = type' }

    let varGet (name : string) : IDivinable =
        DivinableVarGet (name) :> IDivinable

    //static member Value (value : obj) =
    //    DivinableValue (value) :> IDivinable

    //static member Value<'T> (value : 'T) =
    //    DivinableValue (value) :> IDivinable<'T>