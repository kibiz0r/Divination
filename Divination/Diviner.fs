namespace Divination

open System
open System.Reflection
open FSharp.Reflection

[<AutoOpen; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Resolve<'T> (binding : IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            match binding.TryGet<'T> identity with
            | Some entry ->
                entry
            | None ->
                let value =
                    match identity with
                    | Identifier identifier ->
                        this.Identifier (identifier)
                    | CallIdentity (this', methodInfo, arguments) ->
                        let this'' =
                            match this' with
                            | Some t ->
                                let divined = this.Resolve<obj> (binding, t)
                                Some divined
                            | None -> None
                        let arguments' = List.map (fun argument -> this.Resolve<obj> (binding, argument)) arguments
                        this.Call<'T> (this'', methodInfo, arguments')
                    | ConstructorIdentity (constructorInfo, arguments) ->
                        let arguments' = List.map (fun argument -> this.Resolve<obj> (binding, argument)) arguments
                        this.Constructor (constructorInfo, arguments')
                    | PropertyGetIdentity (this', propertyInfo, arguments) ->
                        let this'' =
                            match this' with
                            | Some t -> Some (DivinedValue (t, obj ()))
                            | None -> None
                        let arguments' = List.map (fun argument -> this.Resolve<obj> (binding, argument)) arguments
                        this.PropertyGet (this'', propertyInfo, arguments')
                    | ValueIdentity (value, type') ->
                        this.Value (value, type')
                    | CoerceIdentity (argument, type') ->
                        let argument' = this.Resolve<obj> (binding, argument)
                        this.Coerce (argument', type')
                    | NewUnionCaseIdentity (unionCaseInfo, arguments) ->
                        let arguments' = List.map (fun argument -> this.Resolve<obj> (binding, argument)) arguments
                        this.NewUnionCase (unionCaseInfo, arguments')
                    | VarIdentity (name, value, type') ->
                        this.Var (name, value, type')
                DivinedValue (identity, value)

        member this.ResolveValue<'T> (binding : IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : 'T =
            (this.Resolve<'T> (binding, identity)).Value

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
    default this.Identifier<'T> (identifier) =
        invalidOp "Default Diviner does not handle Identifiers" : 'T

    abstract member Call<'T> : Divined<obj> option * MethodInfo * Divined<obj> list -> 'T
    default this.Call<'T> (this', methodInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> Some t.Value
            | None -> None
        let arguments' = List.map (fun (argument : Divined<obj>) -> argument.Value) arguments |> List.toArray
        methodInfo.Invoke (this'', arguments') :?> 'T

    abstract member Constructor<'T> : ConstructorInfo * Divined<obj> list -> 'T
    default this.Constructor<'T> (constructorInfo, arguments) =
        let arguments' = List.map (fun (argument : Divined<obj>) -> argument.Value) arguments |> List.toArray
        constructorInfo.Invoke (arguments') :?> 'T

    abstract member PropertyGet<'T> : Divined<obj> option * PropertyInfo * Divined<obj> list -> 'T
    default this.PropertyGet<'T> (this', propertyInfo, arguments) =
        let this'' = this'.Value
        let arguments' = List.map (fun (argument : Divined<obj>) -> argument.Value) arguments |> List.toArray
        propertyInfo.GetValue (this'', arguments') :?> 'T

    abstract member Value<'T> : obj * Type -> 'T
    default this.Value<'T> (value, type') =
        Convert.ChangeType (value, type') :?> 'T

    abstract member Coerce<'T> : Divined<obj> * Type -> 'T
    default this.Coerce<'T> (argument, type') =
        let argument' = argument.Value
        argument' :?> 'T
        //Convert.ChangeType (argument', type') :?> 'T

    abstract member NewUnionCase<'T> : UnionCaseInfo * Divined<obj> list -> 'T
    default this.NewUnionCase<'T> (unionCaseInfo, arguments) =
        let arguments' = List.map (fun (argument : Divined<obj>) -> argument.Value) arguments |> List.toArray
        FSharpValue.MakeUnion (unionCaseInfo, arguments') :?> 'T

    abstract member Var<'T> : string * obj * Type -> 'T
    default this.Var<'T> (name, value, type') =
        value :?> 'T

    interface IDiviner with
        member this.Identifier<'T> (identifier) =
            this.Identifier<'T> (identifier)

        member this.Call<'T> (this', methodInfo, arguments) =
            this.Call<'T> (this', methodInfo, arguments)
        
        member this.Constructor<'T> (constructorInfo, arguments) =
            this.Constructor<'T> (constructorInfo, arguments)
        
        member this.PropertyGet<'T> (this', propertyInfo, arguments) =
            this.PropertyGet<'T> (this', propertyInfo, arguments)

        member this.Value<'T> (value, type') =
            this.Value<'T> (value, type')

        member this.Coerce<'T> (argument, type') =
            this.Coerce<'T> (argument, type')

        member this.NewUnionCase<'T> (unionCaseInfo, arguments) =
            this.NewUnionCase<'T> (unionCaseInfo, arguments)

        member this.Var<'T> (name, value, type') =
            this.Var<'T> (name, value, type')