namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open FSharp.Quotations
open FSharp.Quotations.Patterns

type IExprDivinifierBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    abstract member ToDivinableBase : Expr -> IDivinableBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
    abstract member ToDivinableBase : Var -> IDivinableBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

type IExprDivinifierBase<'Identifier, 'Value, 'Type> = IExprDivinifierBase<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IExprDivinifierBase<'Identifier, 'Value> = IExprDivinifierBase<'Identifier, 'Value, Type>

type IExprDivinifierBase<'Identifier> = IExprDivinifierBase<'Identifier, obj>

type IExprDivinifierBase = IExprDivinifierBase<obj>

type IExprDivinifier<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    inherit IExprDivinifierBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
    abstract member ToDivinable<'T> : Expr<'T> -> IDivinable<'T>

type IExprDivinifier<'Identifier, 'Value, 'Type> = IExprDivinifier<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IExprDivinifier<'Identifier, 'Value> = IExprDivinifier<'Identifier, 'Value, Type>

type IExprDivinifier<'Identifier> = IExprDivinifier<'Identifier, obj>

type IExprDivinifier = IExprDivinifier<obj>

type ExprDivinifier (?interceptor : Expr -> IDivinableBase<_> option) =
    interface IExprDivinifierBase<obj, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> with
        member this.ToDivinableBase (expr : Expr) : IDivinableBase<obj, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> =
            let handleNormally () =
                let this' = (this :> IExprDivinifierBase<_, _, _, _, _, _, _>)
                match expr with
                | Let (var, argument, body) ->
                    let var' = this'.ToDivinableBase var
                    let argument' = this'.ToDivinableBase argument
                    let body' = this'.ToDivinableBase body
                    DivinableBase.let' var' argument' body'
                | Value (value, type') ->
                    DivinableBase.value value type'
                | Var (var) ->
                    DivinableBase.var var.Name
                | Call (this'', methodInfo, arguments) ->
                    let this''' =
                        match this'' with
                        | Some t -> Some (this'.ToDivinableBase t)
                        | None -> None
                    let arguments' = List.map (fun (a : Expr) -> this'.ToDivinableBase a) arguments
                    DivinableBase.call this''' methodInfo arguments'
                | NewObject (constructorInfo, arguments) ->
                    let arguments' = List.map (fun (a : Expr) -> this'.ToDivinableBase a) arguments
                    DivinableBase.newObject constructorInfo arguments'
                | _ -> invalidOp (sprintf "Unrecognized expr: %A" expr)
            match interceptor with
            | Some i ->
                match i expr with
                | Some d -> d
                | None -> handleNormally ()
            | None -> handleNormally ()

        member this.ToDivinableBase (var : Var) : IDivinableBase<obj, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> =
            DivinableBase.var var.Name

    interface IExprDivinifier<obj, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> with
        member this.ToDivinable<'T> (expr : Expr<'T>) : IDivinable<'T> =
            (this :> IExprDivinifierBase<_, _, _, _, _, _, _>).ToDivinableBase expr.Raw |> Divinable.cast