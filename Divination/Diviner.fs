namespace Divination

open System
open System.Reflection
open FSharp.Reflection

[<AutoOpen; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    let _ = obj ()

type IFSharpDiviner<'Identifier> =
    inherit IDiviner<'Identifier>

type FSharpDiviner<'Identifier> () =
    static let mutable current : IFSharpDiviner<'Identifier> option = None
    static member Current
        with get () =
            match current with
            | Some c -> c
            | None ->
                let c = FSharpDiviner<'Identifier> () :> IFSharpDiviner<'Identifier>
                current <- Some c
                c
        and set (value) =
            current <- Some value

    interface IFSharpDiviner<'Identifier>

    inherit Diviner<'Identifier> ()

    abstract member Identifier : obj -> obj
    default this.Identifier (identifier) =
        raise (NotImplementedException "FSharpDiviner does not handle Identifiers")

    abstract member Call : Identity option * MethodInfo * Identity list -> obj
    default this.Call (this', methodInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue<obj> (scope, t)
            | None -> null
        let arguments' = List.map (fun (argument : Identity) -> this.ResolveValue<obj> (scope, argument)) arguments |> List.toArray
        methodInfo.Invoke (this'', arguments')

    abstract member NewObject : ConstructorInfo * Identity list -> obj
    default this.NewObject (scope, constructorInfo, arguments) =
        let arguments' = List.map (fun (argument : Identity) -> this.ResolveValue<obj> (scope, argument)) arguments |> List.toArray
        constructorInfo.Invoke (arguments')

    abstract member PropertyGet : Identity option * PropertyInfo * Identity list -> obj
    default this.PropertyGet (scope, this', propertyInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue<obj> (scope, t)
            | None -> null
        let arguments' = List.map (fun (argument : Identity) -> this.ResolveValue<obj> (scope, argument)) arguments |> List.toArray
        propertyInfo.GetValue (this'', arguments')

    abstract member Value : obj * Type -> obj
    default this.Value (value, type') =
        Convert.ChangeType (value, type')

    abstract member Coerce : Identity * Type -> obj
    default this.Coerce (argument, type') =
        Convert.ChangeType (this.Identify argument, type')

    abstract member NewUnionCase : UnionCaseInfo * Identity list -> obj
    default this.NewUnionCase (unionCaseInfo, arguments) =
        let arguments' = List.map this.Identify arguments |> List.toArray
        FSharpValue.MakeUnion (unionCaseInfo, arguments')

    abstract member Var : string -> obj
    default this.Var (name) =
        invalidOp (sprintf "Unbound Var: %s" name)

    abstract member Let : IdentificationScope * Identity * Identity * Identity -> obj
    default this.Let (scope, var, argument, body) =
        this.ResolveValue<'T> (IdentificationScope.add var argument scope, body)

    interface IDiviner with
        member this.Identifier<'T> (scope, identifier) =
            this.Identifier<'T> (scope, identifier)

        member this.Call<'T> (scope, this', methodInfo, arguments) =
            this.Call<'T> (scope, this', methodInfo, arguments)
        
        member this.NewObject<'T> (scope, constructorInfo, arguments) =
            this.NewObject<'T> (scope, constructorInfo, arguments)
        
        member this.PropertyGet<'T> (scope, this', propertyInfo, arguments) =
            this.PropertyGet<'T> (scope, this', propertyInfo, arguments)

        member this.Value<'T> (scope, value, type') =
            this.Value<'T> (scope, value, type')

        member this.Coerce<'T> (scope, argument, type') =
            this.Coerce<'T> (scope, argument, type')

        member this.NewUnionCase<'T> (scope, unionCaseInfo, arguments) =
            this.NewUnionCase<'T> (scope, unionCaseInfo, arguments)

        member this.Var<'T> (scope, name) =
            this.Var<'T> (scope, name)

        member this.Let<'T> (scope, var, argument, body) =
            this.Let<'T> (scope, var, argument, body)