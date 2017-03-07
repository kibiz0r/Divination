namespace Divination

open System
open System.Reflection

[<AutoOpen; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> with
        member this.Resolve (identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>) : 'T =
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

    interface IDiviner with
        member this.Identifier<'T> (_) =
            invalidOp "Default Diviner does not handle Identifiers" : 'T

        member this.Call<'T> (this', methodInfo, arguments) =
            let this'' =
                match this' with
                | Some t -> this.Resolve t
                | None -> null
            let arguments' = List.map this.Resolve arguments |> List.toArray
            methodInfo.Invoke (this', arguments') :?> 'T
        
        member this.Constructor<'T> (constructorInfo, arguments) =
            let arguments' = List.map this.Resolve arguments |> List.toArray
            constructorInfo.Invoke (arguments') :?> 'T
        
        member this.PropertyGet<'T> (this', propertyInfo, arguments) =
            let this'' =
                match this' with
                | Some t -> this.Resolve t
                | None -> null
            let arguments' = List.map this.Resolve arguments |> List.toArray
            propertyInfo.GetValue (this', arguments') :?> 'T

        member this.Value<'T> (value, type') =
            Convert.ChangeType (value, type') :?> 'T
