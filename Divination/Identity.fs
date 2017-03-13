namespace Divination

open System
open System.Collections
open System.Reflection
open FSharp.Reflection

// The concept of Identity is key to the entirety of Divination
[<CustomEquality; CustomComparison>]
type Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    | Identifier of
        'Identifier
    | CallIdentity of
        Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'MethodInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | NewObjectIdentity of
        'ConstructorInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | PropertyGetIdentity of
        Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'PropertyInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | ValueIdentity of
        'Value
        * 'Type
    | CoerceIdentity of
        Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * 'Type
    | NewUnionCaseIdentity of
        'UnionCaseInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | VarIdentity of
        string
    | LetIdentity of
        Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
with
    override this.ToString () =
        sprintf "%A" this

    override this.Equals other =
        match other with
        | :? Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> as o ->
            (this :> IEquatable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>).Equals o
        | _ -> false

    override this.GetHashCode () =
        match this with
        | Identifier i ->
            ("Identifier", i :> obj).GetHashCode ()
        | CallIdentity (t, m, a) ->
            ("CallIdentity", t :> obj, m :> obj, a).GetHashCode ()
        | NewObjectIdentity (c, a) ->
            ("NewObjectIdentity", c :> obj, a).GetHashCode ()
        | PropertyGetIdentity (t, p, a) ->
            ("PropertyGetIdentity", t :> obj, p :> obj, a).GetHashCode ()
        | ValueIdentity (v, t) ->
            ("ValueIdentity", v :> obj, t :> obj).GetHashCode ()
        | CoerceIdentity (a, t) ->
            ("CoerceIdentity", a :> obj, t :> obj).GetHashCode ()
        | NewUnionCaseIdentity (u, a) ->
            ("NewUnionCaseIdentity", u :> obj, a).GetHashCode ()
        | VarIdentity (n) ->
            ("VarIdentity", n :> obj).GetHashCode ()
        | LetIdentity (v, a, b) ->
            ("LetIdentity", v :> obj, a :> obj, b :> obj).GetHashCode ()

    interface IEquatable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>> with
        member this.Equals other =
            match this, other with
            | Identifier i, Identifier i2 ->
                (i :> obj).Equals i2
            | CallIdentity (t, m, a), CallIdentity (t2, m2, a2) ->
                (t :> obj, m :> obj, a).Equals ((t2 :> obj, m2 :> obj, a2))
            | NewObjectIdentity (c, a), NewObjectIdentity (c2, a2) ->
                (c :> obj, a).Equals ((c2 :> obj, a2))
            | PropertyGetIdentity (t, p, a), PropertyGetIdentity (t2, p2, a2) ->
                (t :> obj, p :> obj, a).Equals ((t2 :> obj, p2 :> obj, a2))
            | ValueIdentity (v, t), ValueIdentity (v2, t2) ->
                (v :> obj, t :> obj).Equals ((v2 :> obj, t2 :> obj))
            | CoerceIdentity (a, t), CoerceIdentity (a2, t2) ->
                (a :> obj, t :> obj).Equals ((a2 :> obj, t2 :> obj))
            | NewUnionCaseIdentity (u, a), NewUnionCaseIdentity (u2, a2) ->
                (u :> obj, a).Equals ((u2 :> obj, a2))
            | VarIdentity (n), VarIdentity (n2) ->
                (n :> obj).Equals n2
            | LetIdentity (v, a, b), LetIdentity (v2, a2, b2) ->
                (v :> obj, a :> obj, b :> obj).Equals ((v2 :> obj, a2 :> obj, b2 :> obj))
            | _ -> false

    interface IComparable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>> with
        member this.CompareTo other =
            Comparer.Default.Compare (this.GetHashCode (), other.GetHashCode ())

    interface IComparable with
        member this.CompareTo other =
            (this :> IComparable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>).CompareTo (other :?> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)

type Identity<'Identifier, 'Value, 'Type> = Identity<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type Identity<'Identifier, 'Value> = Identity<'Identifier, 'Value, Type>

type Identity<'Identifier> = Identity<'Identifier, obj>

type Identity = Identity<obj>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Identity =
    let rec cast (identity : Identity<'Identifier, 'Value>) : Identity<'CastIdentifier, 'CastValue> =
        match identity with
        | Identifier identifier ->
            Identifier ((identifier :> obj) :?> 'CastIdentifier)
        | CallIdentity (this, methodInfo, arguments) ->
            let this' =
                match this with
                | Some t -> Some (cast t)
                | None -> None
            let arguments' = List.map cast arguments
            CallIdentity (this', methodInfo, arguments')
        | NewObjectIdentity (constructorInfo, arguments) ->
            let arguments' = List.map cast arguments
            NewObjectIdentity (constructorInfo, arguments')
        | PropertyGetIdentity (this, propertyInfo, arguments) ->
            let this' =
                match this with
                | Some t -> Some (cast t)
                | None -> None
            let arguments' = List.map cast arguments
            PropertyGetIdentity (this', propertyInfo, arguments')
        | ValueIdentity (value, type') ->
            ValueIdentity ((value :> obj) :?> 'CastValue, type')
        | CoerceIdentity (argument, type') ->
            let argument' = cast argument
            CoerceIdentity (argument', type')
        | NewUnionCaseIdentity (unionCaseInfo, arguments) ->
            let arguments' = List.map cast arguments
            NewUnionCaseIdentity (unionCaseInfo, arguments')
        | VarIdentity (name) ->
            VarIdentity (name)
        | LetIdentity (var, argument, body) ->
            LetIdentity (cast var, cast argument, cast body)