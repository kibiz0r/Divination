namespace Divination

open System
open System.Reflection
open FSharp.Reflection

[<AutoOpen; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.ResolveValue (binding : IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : 'T =
            match binding.TryGet identity with
            | Some entry ->
                entry.Value
            | None ->
                match identity with
                | Identifier identifier ->
                    this.Identifier (binding, identifier)
                | CallIdentity (this', methodInfo, arguments) ->
                    this.Call (binding, this', methodInfo, arguments)
                | ConstructorIdentity (constructorInfo, arguments) ->
                    this.Constructor (binding, constructorInfo, arguments)
                | PropertyGetIdentity (this', propertyInfo, arguments) ->
                    this.PropertyGet (binding, this', propertyInfo, arguments)
                | ValueIdentity (value, type') ->
                    this.Value (binding, value, type')
                | CoerceIdentity (argument, type') ->
                    this.Coerce (binding, argument, type')
                | NewUnionCaseIdentity (unionCaseInfo, arguments) ->
                    this.NewUnionCase (binding, unionCaseInfo, arguments)

        member this.Resolve (binding : IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, _, _, _, _, _, _, _> =
            let value = this.ResolveValue (binding, identity)
            DivinedValue (identity, value)

type Diviner () =
    static let mutable current : IDiviner option = None
    static member Current
        with get () =
            match current with
            | Some c -> c
            | None ->
                let c = Diviner () :> IDiviner
                current <- Some c
                c
        and set (value) =
            current <- Some value

    abstract member Identifier<'T> : IDivinationBinding * obj -> 'T
    default this.Identifier<'T> (binding : IDivinationBinding, identifier : obj) =
        invalidOp "Default Diviner does not handle Identifiers" : 'T

    abstract member Call<'T> : IDivinationBinding * Identity option * MethodInfo * Identity list -> 'T
    default this.Call<'T> (binding, this', methodInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue (binding, t)
            | None -> null
        let arguments' = List.map (fun argument -> this.ResolveValue (binding, argument)) arguments |> List.toArray
        methodInfo.Invoke (this'', arguments') :?> 'T

    abstract member Constructor<'T> : IDivinationBinding * ConstructorInfo * Identity list -> 'T
    default this.Constructor<'T> (binding, constructorInfo, arguments) =
        let arguments' = List.map (fun argument -> this.ResolveValue (binding, argument)) arguments |> List.toArray
        constructorInfo.Invoke (arguments') :?> 'T

    abstract member PropertyGet<'T> : IDivinationBinding * Identity option * PropertyInfo * Identity list -> 'T
    default this.PropertyGet<'T> (binding, this', propertyInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue (binding, t)
            | None -> null
        let arguments' = List.map (fun argument -> this.ResolveValue (binding, argument)) arguments |> List.toArray
        propertyInfo.GetValue (this'', arguments') :?> 'T

    abstract member Value<'T> : IDivinationBinding * obj * Type -> 'T
    default this.Value (binding, value, type') =
        Convert.ChangeType (value, type') :?> 'T

    abstract member Coerce<'T> : IDivinationBinding * Identity * Type -> 'T
    default this.Coerce (binding, argument, type') =
        let argument' = this.ResolveValue (binding, argument)
        (argument' :> obj) :?> 'T
        //Convert.ChangeType (argument', type') :?> 'T

    abstract member NewUnionCase<'T> : IDivinationBinding * UnionCaseInfo * Identity list -> 'T
    default this.NewUnionCase (binding, unionCaseInfo, arguments) =
        let arguments' = List.map (fun argument -> this.ResolveValue (binding, argument)) arguments |> List.toArray
        FSharpValue.MakeUnion (unionCaseInfo, arguments') :?> 'T

    interface IDiviner with
        member this.Identifier<'T> (binding, identifier) =
            this.Identifier<'T> (binding, identifier)

        member this.Call<'T> (binding, this', methodInfo, arguments) =
            this.Call<'T> (binding, this', methodInfo, arguments)
        
        member this.Constructor<'T> (binding, constructorInfo, arguments) =
            this.Constructor<'T> (binding, constructorInfo, arguments)
        
        member this.PropertyGet<'T> (binding, this', propertyInfo, arguments) =
            this.PropertyGet<'T> (binding, this', propertyInfo, arguments)

        member this.Value<'T> (binding, value, type') =
            this.Value<'T> (binding, value, type')

        member this.Coerce<'T> (binding, argument, type') =
            this.Coerce<'T> (binding, argument, type')

        member this.NewUnionCase<'T> (binding, unionCaseInfo, arguments) =
            this.NewUnionCase<'T> (binding, unionCaseInfo, arguments)