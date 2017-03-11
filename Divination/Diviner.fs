namespace Divination

open System
open System.Reflection
open FSharp.Reflection

[<AutoOpen; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Resolve<'T> (scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            let resolveValue (identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : 'T =
                    match identity with
                    | Identifier identifier ->
                        this.Identifier (scope, identifier)
                    | CallIdentity (this', methodInfo, arguments) ->
                        this.Call<'T> (scope, this', methodInfo, arguments)
                    | ConstructorIdentity (constructorInfo, arguments) ->
                        this.Constructor<'T> (scope, constructorInfo, arguments)
                    | PropertyGetIdentity (this', propertyInfo, arguments) ->
                        this.PropertyGet<'T> (scope, this', propertyInfo, arguments)
                    | ValueIdentity (value, type') ->
                        this.Value<'T> (scope, value, type')
                    | CoerceIdentity (argument, type') ->
                        this.Coerce<'T> (scope, argument, type')
                    | NewUnionCaseIdentity (unionCaseInfo, arguments) ->
                        this.NewUnionCase<'T> (scope, unionCaseInfo, arguments)
                    | VarIdentity (name, type') ->
                        this.Var<'T> (scope, name, type')
                    | LetIdentity (var, argument, body) ->
                        this.Let<'T> (scope, var, argument, body)
            match IdentificationScope.tryFind identity scope with
            | Some bound ->
                let value = resolveValue bound
                DivinedValue (bound, value)
            | None ->
                let value = resolveValue identity
                DivinedValue (identity, value)

        member this.ResolveValue<'T> (scope : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : 'T =
            (this.Resolve<'T> (scope, identity)).Value

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

    abstract member Call<'T> : IdentificationScope * Identity option * MethodInfo * Identity list -> 'T
    default this.Call<'T> (scope, this', methodInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue<obj> (scope, t)
            | None -> null
        let arguments' = List.map (fun (argument : Identity) -> this.ResolveValue<obj> (scope, argument)) arguments |> List.toArray
        methodInfo.Invoke (this'', arguments') :?> 'T

    abstract member Constructor<'T> : IdentificationScope * ConstructorInfo * Identity list -> 'T
    default this.Constructor<'T> (scope, constructorInfo, arguments) =
        let arguments' = List.map (fun (argument : Identity) -> this.ResolveValue<obj> (scope, argument)) arguments |> List.toArray
        constructorInfo.Invoke (arguments') :?> 'T

    abstract member PropertyGet<'T> : IdentificationScope * Identity option * PropertyInfo * Identity list -> 'T
    default this.PropertyGet<'T> (scope, this', propertyInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.ResolveValue<obj> (scope, t)
            | None -> null
        let arguments' = List.map (fun (argument : Identity) -> this.ResolveValue<obj> (scope, argument)) arguments |> List.toArray
        propertyInfo.GetValue (this'', arguments') :?> 'T

    abstract member Value<'T> : IdentificationScope * obj * Type -> 'T
    default this.Value<'T> (scope, value, type') =
        Convert.ChangeType (value, type') :?> 'T

    abstract member Coerce<'T> : IdentificationScope * Identity * Type -> 'T
    default this.Coerce<'T> (scope, argument, type') =
        let argument' = this.ResolveValue<obj> (scope, argument)
        argument' :?> 'T
        //Convert.ChangeType (argument', type') :?> 'T

    abstract member NewUnionCase<'T> : IdentificationScope * UnionCaseInfo * Identity list -> 'T
    default this.NewUnionCase<'T> (scope, unionCaseInfo, arguments) =
        let arguments' = List.map (fun (argument : Identity) -> this.ResolveValue<obj> (scope, argument)) arguments |> List.toArray
        FSharpValue.MakeUnion (unionCaseInfo, arguments') :?> 'T

    abstract member Var<'T> : IdentificationScope * string * Type -> 'T
    default this.Var<'T> (scope, name, type') : 'T =
        invalidOp (sprintf "Unbound Var %s : %A" name type')

    abstract member Let<'T> : IdentificationScope * Identity * Identity * Identity -> 'T
    default this.Let<'T> (scope, var, argument, body) : 'T =
        this.ResolveValue<'T> (IdentificationScope.add var argument scope, body)

    interface IDiviner with
        member this.Identifier<'T> (scope, identifier) =
            this.Identifier<'T> (scope, identifier)

        member this.Call<'T> (scope, this', methodInfo, arguments) =
            this.Call<'T> (scope, this', methodInfo, arguments)
        
        member this.Constructor<'T> (scope, constructorInfo, arguments) =
            this.Constructor<'T> (scope, constructorInfo, arguments)
        
        member this.PropertyGet<'T> (scope, this', propertyInfo, arguments) =
            this.PropertyGet<'T> (scope, this', propertyInfo, arguments)

        member this.Value<'T> (scope, value, type') =
            this.Value<'T> (scope, value, type')

        member this.Coerce<'T> (scope, argument, type') =
            this.Coerce<'T> (scope, argument, type')

        member this.NewUnionCase<'T> (scope, unionCaseInfo, arguments) =
            this.NewUnionCase<'T> (scope, unionCaseInfo, arguments)

        member this.Var<'T> (scope, name, type') =
            this.Var<'T> (scope, name, type')

        member this.Let<'T> (scope, var, argument, body) =
            this.Let<'T> (scope, var, argument, body)