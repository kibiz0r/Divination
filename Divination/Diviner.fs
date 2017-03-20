namespace Divination

open System
open System.Reflection
open FSharp.Reflection

[<AbstractClass>]
type Diviner<'Identifier> () =
    interface IDiviner<'Identifier> with
        member this.Resolve (scope, identity) =
            this.Resolve (scope, identity)

        member this.Resolve<'T> (scope, identity) =
            this.Resolve<'T> (scope, identity)

        member this.NewContext (scope) =
            this.NewContext scope

        member this.Canonicalize (scope, contextualIdentity) =
            this.Canonicalize (scope, contextualIdentity)

    abstract member Resolve : DivinationScope<'Identifier>
        * Identity<'Identifier>
        -> obj
    default this.Resolve (scope, identity) =
        match Map.tryFind identity scope.ResolutionMap with
        | Some r -> r
        | None ->
            match IdentificationScope.tryFind identity scope.IdentificationScope with
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

    abstract member Resolve<'T> : DivinationScope<'Identifier>
        * Identity<'Identifier>
        -> 'T
    default this.Resolve<'T> (scope, identity) : 'T =
        this.Resolve (scope, identity) :?> 'T

    abstract member NewContext : DivinationScope<'Identifier>
        -> IDivinationContext<'Identifier>
    default this.NewContext (scope) =
        DivinationContext<'Identifier> (this, scope) :> _

    abstract member Canonicalize : DivinationScope<'Identifier>
        * ContextualIdentity<'Identifier>
        -> DivinationScope<'Identifier> * Identity<'Identifier>
    default this.Canonicalize (scope, contextualIdentity) =
        match IdentificationScope.tryFind contextualIdentity.Identity contextualIdentity.Scope.IdentificationScope with
        | Some i ->
            contextualIdentity.Scope, i
        | None ->
            contextualIdentity.Scope, contextualIdentity.Identity

    abstract member Identifier : DivinationScope<'Identifier>
        * 'Identifier
        -> obj

    abstract member Call : DivinationScope<'Identifier>
        * Identity<'Identifier> option
        * MethodInfo
        * Identity<'Identifier> list
        -> obj
    
    abstract member NewObject : DivinationScope<'Identifier>
        * ConstructorInfo
        * Identity<'Identifier> list
        -> obj
    
    abstract member PropertyGet : DivinationScope<'Identifier>
        * Identity<'Identifier> option
        * PropertyInfo
        * Identity<'Identifier> list
        -> obj

    abstract member Value : DivinationScope<'Identifier>
        * obj
        * Type
        -> obj

    abstract member Coerce : DivinationScope<'Identifier>
        * Identity<'Identifier>
        * Type
        -> obj

    abstract member NewUnionCase : DivinationScope<'Identifier>
        * UnionCaseInfo
        * Identity<'Identifier> list
        -> obj

    abstract member Var : DivinationScope<'Identifier>
        * string
        -> obj

    abstract member Let : DivinationScope<'Identifier>
        * Identity<'Identifier>
        * Identity<'Identifier>
        * Identity<'Identifier>
        -> obj
