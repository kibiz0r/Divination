namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open FSharp.Quotations

type IExprIdentifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    abstract member ToIdentity : Expr -> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

type IExprIdentifier<'Identifier, 'Value, 'Type> = IExprIdentifier<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IExprIdentifier<'Identifier, 'Value> = IExprIdentifier<'Identifier, 'Value, Type>

type IExprIdentifier<'Identifier> = IExprIdentifier<'Identifier, obj>

type IExprIdentifier = IExprIdentifier<obj>

type ExprIdentifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> () =
    static let mutable current : IExprIdentifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option = None
    static member Current
        with get () =
            match current with
            | Some c -> c
            | None ->
                let c = ExprIdentifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> () :> IExprIdentifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
                current <- Some c
                c
        and set (value) =
            current <- Some value

    static member ToIdentity (expr : Expr) : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
        ExprIdentifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>.Current.ToIdentity (expr)

    interface IExprIdentifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.ToIdentity (expr : Expr) : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            ValueIdentity (Unchecked.defaultof<'Value>, Unchecked.defaultof<'Type>)

type ExprIdentifier<'Identifier, 'Value, 'Type> = ExprIdentifier<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type ExprIdentifier<'Identifier, 'Value> = ExprIdentifier<'Identifier, 'Value, Type>

type ExprIdentifier<'Identifier> = ExprIdentifier<'Identifier, obj>

type ExprIdentifier = ExprIdentifier<obj>