﻿namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

// DivineBuilder wraps a DivinableBuilder and invokes it with the current Diviner and an empty DivinationBinding
type DivineBuilder (divinableBuilder : DivinableBuilder) =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>) : IDivinable<'U> =
        obj () :?> IDivinable<'U>
        //Divinable.expr divinableExpr
        //|> Divinable.unwrap
        //|> Divinable.bind body

    member this.Return ([<ReflectedDefinition>] returnExpr : Expr<'T>) : IDivinable<'T> =
        let rec exprToIdentity expr =
            match expr with
            | NewObject (constructorInfo, arguments) ->
                let arguments' = List.map exprToIdentity arguments
                ConstructorIdentity (constructorInfo, arguments')
            | _ ->
                invalidOp (returnExpr.ToString ())
        Divinable<'T> (fun diviner -> exprToIdentity returnExpr) :> IDivinable<'T>
