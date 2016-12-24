namespace Divination

open System
open System.Reflection
open FSharp.Quotations
open FSharp.Interop.Dynamic

// F# Expr: has more power than we can hope to translate, and also takes things too literally
// |> Divinable: limited to the only what we can express when fully materialized into the runtime, but speaks in terms of Exprs and IDivinables and incorporates hooks that allow custom expression nodes
// |> IDivinable: totally flexible, but must produce a DivinedExpr when given a Diviner (or maybe something related, like a DivineContext?), which that Diviner will then evaluate
// |> DivinedExpr: the complete, serializable (primitive data only) expression tree, which represents the actual path of execution to generate a Divined value; the same DivinedExpr must always produce the same Divined value

module Divinable =
    type internal Divinable<'T> = {
        Raw : IDivinable
    } with
        interface IDivinable<'T> with
            member this.Raw = this.Raw

        interface IDivinable with
            member this.DivineExpr () = this.Raw.DivineExpr ()

    let divine (diviner : IDiviner<'Expr, 'Context>) (context : 'Context) (divinable : IDivinable<'T>) : Divined<'T> =
        let divineExpr = divinable.DivineExpr () :?> 'Expr
        let value = diviner.Eval (divineExpr, context)

        {
            Source = divineExpr :> IDivineExpr
            Value = value :?> 'T
        }

    let cast (raw : IDivinable) : IDivinable<'T> =
        ({ Raw = raw } : Divinable<'T>) :> IDivinable<'T>

//[<StructuralEquality; StructuralComparison>]
//type DivinedVar = {
//    Name : string
//    TypeName : string
//}

//type DivinedExpr =
//    | DivinedCall of DivinedCallExpr
//    | DivinedDivinerCall of DivinedDivinerCallExpr
//    | DivinedLet of DivinedLetExpr
//    | DivinedValue of DivinedValueExpr
//    | DivinedVarGet of DivinedVarGetExpr

//and DivinedCallExpr = {
//    This : DivinedExpr option
//    TypeName : string
//    MethodName : string
//    Arguments : DivinedExpr list
//}

//and DivinedDivinerCallExpr = {
//    MethodName : string
//    Arguments : DivinedExpr list
//}

//and DivinedLetExpr = {
//    Var : DivinedVar
//    Value : DivinedExpr
//    Body : DivinedExpr
//}

//and DivinedValueExpr = {
//    Value : obj
//    TypeName : string
//}

//and DivinedVarGetExpr = {
//    Var : DivinedVar
//}

//type DivinableVar = {
//    Name : string
//    TypeName : string
//} with
//    interface IDivinableVar with
//        member this.Name = this.Name

//type DivinableCall (this' : IDivinable option, typeName : string, methodName : string, arguments : IDivinable list) =
//    member this.This = this'
//    member this.TypeName = typeName
//    member this.MethodName = methodName
//    member this.Arguments = arguments

//    override this.GetHashCode() =
//        hash (this', methodName, arguments)

//    override this.Equals(other) =
//        match other with
//        | :? DivinableCall as o -> (this', typeName, methodName, arguments) = (o.This, o.TypeName, o.MethodName, o.Arguments)
//        | _ -> false

//    interface IDivinable with
//        member this.DivineExpr diviner =
//            let this'' =
//                match this' with
//                | Some t -> Some (t.DivineExpr diviner)
//                | None -> None
//            let arguments' = arguments |> List.map (fun a -> a.DivineExpr diviner)
//            DivinedExpr.DivinedCall { This = this''; TypeName = typeName; MethodName = methodName; Arguments = arguments' }

//type DivinableExaltedCall (this' : obj option, type' : Type, methodName : string, arguments : IDivinable list) =
//    member this.This = this'
//    member this.Type = type'
//    member this.MethodName = methodName
//    member this.Arguments = arguments

//    override this.GetHashCode() =
//        hash (this', methodName, arguments)

//    override this.Equals(other) =
//        match other with
//        | :? DivinableExaltedCall as o -> (this', type', methodName, arguments) = (o.This, o.Type, o.MethodName, o.Arguments)
//        | _ -> false

//    interface IDivinable with
//        member this.DivineExpr diviner =
//            let this'' =
//                match this' with
//                | Some t -> t
//                | None -> null
//            let method' = type'.GetMethod methodName
//            let arguments' = arguments |> Seq.cast |> Seq.toArray
//            let divinable = method'.Invoke (this'', arguments') :?> IDivinable
//            divinable.DivineExpr diviner

//type DivinableLet (var : DivinableVar, value : IDivinable, body : IDivinable) =
//    member this.Var = var
//    member this.Value = value
//    member this.Body = body

//    override this.GetHashCode() =
//        hash (var, value, body)

//    override this.Equals(other) =
//        match other with
//        | :? DivinableLet as o -> (var, value, body) = (o.Var, o.Value, o.Body)
//        | _ -> false

//    interface IDivinable with
//        member this.DivineExpr diviner =
//            let var' : DivinedVar = { Name = var.Name; TypeName = var.TypeName }
//            let value' = value.DivineExpr diviner
//            let body' = body.DivineExpr diviner
//            DivinedExpr.DivinedLet { Var = var'; Value = value'; Body = body' }

//type DivinableValue (value : obj, typeName : string) =
//    member this.Value = value
//    member this.TypeName = typeName

//    override this.GetHashCode() =
//        hash (value, typeName)

//    override this.Equals(other) =
//        match other with
//        | :? DivinableValue as o -> (value, typeName) = (o.Value, o.TypeName)
//        | _ -> false

//    interface IDivinable with
//        member this.DivineExpr diviner =
//            DivinedExpr.DivinedValue { Value = value; TypeName = typeName }

//type DivinableVarGet (var : DivinableVar) =
//    member this.Var = var

//    override this.GetHashCode() =
//        hash var

//    override this.Equals(other) =
//        match other with
//        | :? DivinableVarGet as o -> var = o.Var
//        | _ -> false

//    interface IDivinable with
//        member this.DivineExpr diviner =
//            DivinedExpr.DivinedVarGet { Var = { Name = var.Name; TypeName = var.TypeName } }


    //let call (this : IDivinable option, typeName : string, methodName : string, arguments : IDivinable list) : IDivinable =
    //    DivinableCall (this, typeName, methodName, arguments) :> IDivinable

    //let exaltedCall (this : obj option, type' : Type, methodName : string, arguments : IDivinable list) : IDivinable =
    //    DivinableExaltedCall (this, type', methodName, arguments) :> IDivinable

    //let let' (var : DivinableVar, value : IDivinable, body : IDivinable) : IDivinable =
    //    DivinableLet (var, value, body) :> IDivinable

    //let value (value : obj, typeName : string) : IDivinable =
    //    DivinableValue (value, typeName) :> IDivinable

    //let var (name : string, typeName : string) : DivinableVar =
    //    { Name = name; TypeName = typeName }

    //let varGet (var : DivinableVar) : IDivinable =
    //    DivinableVarGet (var) :> IDivinable

    //static member Value (value : obj) =
    //    DivinableValue (value) :> IDivinable

    //static member Value<'T> (value : 'T) =
    //    DivinableValue (value) :> IDivinable<'T>