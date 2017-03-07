﻿namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () =
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
            | Value (value, type') ->
                ValueIdentity (value, type')
            | _ ->
                invalidOp (expr.ToString ())
        Divinable<'T> (fun diviner -> exprToIdentity returnExpr) :> IDivinable<'T>