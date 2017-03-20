namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open FSharp.Quotations
open FSharp.Quotations.Patterns

type ExprDivinifier<'Identifier> (?interceptor : Expr -> IDivinableBase<DivinationContext<'Identifier>, 'Identifier, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> option) =
    member this.ToDivinableBase (expr : Expr) : IDivinableBase<DivinationContext<'Identifier>, 'Identifier, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> =
        Unchecked.defaultof<IDivinableBase<DivinationContext<'Identifier>, 'Identifier, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>>

    member this.ToDivinableBase (var : Var) : IDivinableBase<DivinationContext<'Identifier>, 'Identifier, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> =
        Unchecked.defaultof<IDivinableBase<DivinationContext<'Identifier>, 'Identifier, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>>

    member this.ToDivinable<'T> (expr : Expr<'T>) : IDivinable<'T, 'Identifier> =
        Unchecked.defaultof<IDivinable<'T, 'Identifier>>
        //this.ToDivinableBase expr.Raw |> Divinable.cast

    interface IExprDivinifierBase<DivinationContext<'Identifier>, 'Identifier, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo> with
        member this.ToDivinableBase (expr : Expr) =
            this.ToDivinableBase expr
            //let handleNormally () =
            //    let this' = (this :> IExprDivinifierBase<_, _, _, _, _, _, _>)
            //    match expr with
            //    | Let (var, argument, body) ->
            //        let var' = this'.ToDivinableBase var
            //        let argument' = this'.ToDivinableBase argument
            //        let body' = this'.ToDivinableBase body
            //        DivinableBase.let' var' argument' body'
            //    | Value (value, type') ->
            //        DivinableBase.value value type'
            //    | Var (var) ->
            //        DivinableBase.var var.Name
            //    | Call (this'', methodInfo, arguments) ->
            //        let this''' =
            //            match this'' with
            //            | Some t -> Some (this'.ToDivinableBase t)
            //            | None -> None
            //        let arguments' = List.map (fun (a : Expr) -> this'.ToDivinableBase a) arguments
            //        DivinableBase.call this''' methodInfo arguments'
            //    | NewObject (constructorInfo, arguments) ->
            //        let arguments' = List.map (fun (a : Expr) -> this'.ToDivinableBase a) arguments
            //        DivinableBase.newObject constructorInfo arguments'
            //    | _ -> invalidOp (sprintf "Unrecognized expr: %A" expr)
            //match interceptor with
            //| Some i ->
            //    match i expr with
            //    | Some d -> d
            //    | None -> handleNormally ()
            //| None -> handleNormally ()

        member this.ToDivinableBase (var : Var) =
            this.ToDivinableBase var
            //DivinableBase.var var.Name

    interface IExprDivinifier<'Identifier> with
        member this.ToDivinable<'T> (expr) =
            this.ToDivinable<'T> expr