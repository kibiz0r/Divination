namespace Divination

open System
open System.Reflection
open FSharp.Reflection

[<AbstractClass>]
type Diviner<'Identifier> () =
    //member this.DivineBase (scope : IdentificationScope<'Identifier>, divinable : IDivinableBase<DivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    //    let newContext = this.NewContext scope
    //    let contextualized = divinable.Contextualize newContext
    //    let identity = this.EvaluateContext contextualized
    //    let value = this.Resolve (scope, identity)
    //    DivinedValue (identity, value)

    //member this.Divine<'T> (scope : IdentificationScope<'Identifier>, divinable : IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    //    this.DivineBase (scope, divinable :> IDivinableBase<DivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)
    //    |> Divined.cast

    interface IDiviner<'Identifier> with
        member this.NewContext (scope) =
            this.NewContext scope

        member this.EvaluateContext (context) =
            this.EvaluateContext context

        member this.Resolve (scope, identity) =
            this.Resolve (scope, identity)

        member this.Resolve<'T> (scope : IdentificationScope<'Identifier>, identity : Identity<'Identifier>) =
            this.Resolve<'T> (scope, identity)

    abstract member NewContext : IdentificationScope<'Identifier>
        -> DivinationContext<'Identifier>

    abstract member EvaluateContext : DivinationContext<'Identifier>
        -> Identity<'Identifier>

    abstract member Resolve : IdentificationScope<'Identifier>
        * Identity<'Identifier>
        -> obj
    default this.Resolve (scope, identity) =
        match IdentificationScope.tryFind identity scope with
        | Some i -> this.Resolve (scope, i)
        | None ->
            match identity with
            | Identifier identifier ->
                this.Identifier (scope, identifier)
            | CallIdentity (this', methodInfo, arguments) ->
                this.Call (scope, this', methodInfo, arguments)
            | NewObjectIdentity (constructorInfo, arguments) ->
                this.NewObject (scope, constructorInfo, arguments)
            | PropertyGetIdentity (this', propertyInfo, arguments) ->
                this.PropertyGet (scope, this', propertyInfo, arguments)
            | ValueIdentity (value, type') ->
                this.Value (scope, value, type')
            | CoerceIdentity (argument, type') ->
                this.Coerce (scope, argument, type')
            | NewUnionCaseIdentity (unionCaseInfo, arguments) ->
                this.NewUnionCase (scope, unionCaseInfo, arguments)
            | VarIdentity name ->
                this.Var (scope, name)
            | LetIdentity (var, argument, body) ->
                this.Let (scope, var, argument, body)

    abstract member Resolve<'T> : IdentificationScope<'Identifier>
        * Identity<'Identifier>
        -> 'T
    default this.Resolve<'T> (scope, identity) : 'T =
        this.Resolve (scope, identity) :?> 'T

    abstract member Identifier : IdentificationScope<'Identifier>
        * 'Identifier
        -> obj

    abstract member Call : IdentificationScope<'Identifier>
        * Identity<'Identifier> option
        * MethodInfo
        * Identity<'Identifier> list
        -> obj
    
    abstract member NewObject : IdentificationScope<'Identifier>
        * ConstructorInfo
        * Identity<'Identifier> list
        -> obj
    
    abstract member PropertyGet : IdentificationScope<'Identifier>
        * Identity<'Identifier> option
        * PropertyInfo
        * Identity<'Identifier> list
        -> obj

    abstract member Value : IdentificationScope<'Identifier>
        * obj
        * Type
        -> obj

    abstract member Coerce : IdentificationScope<'Identifier>
        * Identity<'Identifier>
        * Type
        -> obj

    abstract member NewUnionCase : IdentificationScope<'Identifier>
        * UnionCaseInfo
        * Identity<'Identifier> list
        -> obj

    abstract member Var : IdentificationScope<'Identifier>
        * string
        -> obj

    abstract member Let : IdentificationScope<'Identifier>
        * Identity<'Identifier>
        * Identity<'Identifier>
        * Identity<'Identifier>
        -> obj
