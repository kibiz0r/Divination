namespace Divination

open System
open System.Reflection

[<AutoOpen; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> with
        member this.ResolveValue (identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>) : 'T =
            match identity with
            | Identifier identifier ->
                this.Identifier identifier
            | CallIdentity (this', methodInfo, arguments) ->
                this.Call (this', methodInfo, arguments)
            | ConstructorIdentity (constructorInfo, arguments) ->
                this.Constructor (constructorInfo, arguments)
            | PropertyGetIdentity (this', propertyInfo, arguments) ->
                this.PropertyGet (this', propertyInfo, arguments)
            | ValueIdentity (value, type') ->
                this.Value (value, type')

        member this.Resolve (identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>) : Divined<'T, _, _, _, _, _, _> =
            let value = this.ResolveValue identity
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

    abstract member Identifier<'T> : obj -> 'T
    default this.Identifier<'T> (_) =
        invalidOp "Default Diviner does not handle Identifiers" : 'T

    abstract member Call<'T> : Identity option * MethodInfo * Identity list -> 'T
    default this.Call<'T> (this', methodInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue t
            | None -> null
        let arguments' = List.map this.ResolveValue arguments |> List.toArray
        methodInfo.Invoke (this', arguments') :?> 'T

    abstract member Constructor<'T> : ConstructorInfo * Identity list -> 'T
    default this.Constructor<'T> (constructorInfo, arguments) =
        let arguments' = List.map this.ResolveValue arguments |> List.toArray
        constructorInfo.Invoke (arguments') :?> 'T

    abstract member PropertyGet<'T> : Identity option * PropertyInfo * Identity list -> 'T
    default this.PropertyGet<'T> (this', propertyInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue t
            | None -> null
        let arguments' = List.map this.ResolveValue arguments |> List.toArray
        propertyInfo.GetValue (this', arguments') :?> 'T

    abstract member Value<'T> : obj * Type -> 'T
    default this.Value (value, type') =
        Convert.ChangeType (value, type') :?> 'T

    interface IDiviner with
        member this.Identifier<'T> (identifier) =
            this.Identifier<'T> identifier

        member this.Call<'T> (this', methodInfo, arguments) =
            this.Call<'T> (this', methodInfo, arguments)
        
        member this.Constructor<'T> (constructorInfo, arguments) =
            this.Constructor<'T> (constructorInfo, arguments)
        
        member this.PropertyGet<'T> (this', propertyInfo, arguments) =
            this.PropertyGet<'T> (this', propertyInfo, arguments)

        member this.Value<'T> (value, type') =
            this.Value<'T> (value, type')
