namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open FSharp.Quotations

type IExprIdentifier<'Identifier> =
    abstract member ToIdentity : Expr -> Identity<'Identifier>

type ExprIdentifier<'Identifier> () =
    static let mutable current : IExprIdentifier<'Identifier> option = None
    static member Current
        with get () =
            match current with
            | Some c -> c
            | None ->
                let c = ExprIdentifier<'Identifier> () :> IExprIdentifier<'Identifier>
                current <- Some c
                c
        and set (value) =
            current <- Some value

    static member ToIdentity (expr : Expr) : Identity<'Identifier> =
        ExprIdentifier<'Identifier>.Current.ToIdentity (expr)

    interface IExprIdentifier<'Identifier> with
        member this.ToIdentity (expr : Expr) : Identity<'Identifier> =
            ValueIdentity (obj (), typeof<obj>)