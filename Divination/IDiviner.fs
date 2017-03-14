namespace Divination

open System
open System.Reflection
open FSharp.Reflection

// A Diviner can attempt to resolve any given Identity plus Type to an instance of that Type.
type IDivinerBase<'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    abstract member DivineBase : IDivinableBase<'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        -> IDivinedBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    inherit IDivinerBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

    abstract member Divine<'T> : IDivinable<'T, IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        -> IDivined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

[<AbstractClass>]
type Diviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    interface IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Divine<'T> (divinable : IDivinable<'T, IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : IDivined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            this.DivineBase (divinable :> _) |> Divined.cast

        member this.DivineBase (divinable) =
            let identity = this.NewContext () |> divinable.Contextualize |> this.ResolveContext
            let value = this.Identify identity
            DivinedValueBase (identity, value) :> _

    abstract member NewContext : unit
        -> IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

    abstract member ResolveContext : IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        -> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

    abstract member Identify : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        -> obj
    default this.Identify (scope, identity) =
        match identity with
        | Identifier identifier ->
            this.Identifier identifier
        | CallIdentity (this', methodInfo, arguments) ->
            this.Call (this', methodInfo, arguments)
        | NewObjectIdentity (constructorInfo, arguments) ->
            this.NewObject (constructorInfo, arguments)
        | PropertyGetIdentity (this', propertyInfo, arguments) ->
            this.PropertyGet (this', propertyInfo, arguments)
        | ValueIdentity (value, type') ->
            this.Value (value, type')
        | CoerceIdentity (argument, type') ->
            this.Coerce (argument, type')
        | NewUnionCaseIdentity (unionCaseInfo, arguments) ->
            this.NewUnionCase (unionCaseInfo, arguments)
        | VarIdentity name ->
            this.Var name
        | LetIdentity (var, argument, body) ->
            this.Let (var, argument, body)

    // FIXME: I think these all still need a concept of scope...
    abstract member Identifier : 'Identifier
        -> obj

    abstract member Call : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'MethodInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> obj
    
    abstract member NewObject : 'ConstructorInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> obj
    
    abstract member PropertyGet : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'PropertyInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> obj

    abstract member Value : 'Value
        * 'Type
        -> obj

    abstract member Coerce : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * 'Type
        -> obj

    abstract member NewUnionCase : 'UnionCaseInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> obj

    abstract member Var : string
        -> obj

    abstract member Let : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        -> obj

type IDiviner<'Identifier, 'Value, 'Type> = IDiviner<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IDiviner<'Identifier, 'Value> = IDiviner<'Identifier, 'Value, Type>

type IDiviner<'Identifier> = IDiviner<'Identifier, obj>

type IDiviner = IDiviner<obj>