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
    | ConstructorIdentity of
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
        * 'Type
with
    override this.Equals other =
        match other with
        | :? Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> as o ->
            (this :> IEquatable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>).Equals o
        | _ -> false

    override this.GetHashCode () =
        match this with
        | Identifier i ->
            (i :> obj).GetHashCode ()
        | CallIdentity (t, m, a) ->
            (t :> obj, m :> obj, a).GetHashCode ()
        | ConstructorIdentity (c, a) ->
            (c :> obj, a).GetHashCode ()
        | PropertyGetIdentity (t, p, a) ->
            (t :> obj, p :> obj, a).GetHashCode ()
        | ValueIdentity (v, t) ->
            (v :> obj, t :> obj).GetHashCode ()
        | CoerceIdentity (a, t) ->
            (a :> obj, t :> obj).GetHashCode ()
        | NewUnionCaseIdentity (u, a) ->
            (u :> obj, a).GetHashCode ()
        | VarIdentity (n, t) ->
            (n :> obj, t :> obj).GetHashCode ()

    interface IEquatable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>> with
        member this.Equals other =
            match this, other with
            | Identifier i, Identifier i2 ->
                (i :> obj).Equals i2
            | CallIdentity (t, m, a), CallIdentity (t2, m2, a2) ->
                (t :> obj, m :> obj, a).Equals ((t2 :> obj, m2 :> obj, a2))
            | ConstructorIdentity (c, a), ConstructorIdentity (c2, a2) ->
                (c :> obj, a).Equals ((c2 :> obj, a2))
            | PropertyGetIdentity (t, p, a), PropertyGetIdentity (t2, p2, a2) ->
                (t :> obj, p :> obj, a).Equals ((t2 :> obj, p2 :> obj, a2))
            | CoerceIdentity (a, t), CoerceIdentity (a2, t2) ->
                (a :> obj, t :> obj).Equals ((a2 :> obj, t2 :> obj))
            | NewUnionCaseIdentity (u, a), NewUnionCaseIdentity (u2, a2) ->
                (u :> obj, a).Equals ((u2 :> obj, a2))
            | VarIdentity (n, t), VarIdentity (n2, t2) ->
                (n :> obj, t :> obj).Equals ((n2 :> obj, t2 :> obj))
            | _ -> false

    interface IComparable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>> with
        member this.CompareTo other =
            let comparisonObj identity =
                identity.GetHashCode ()
                //match identity with
                //| Identifier i ->
                //    i :> obj
                //| CallIdentity (t, m, a) ->
                //    (t, m, a) :> obj
                //| ConstructorIdentity (t, a) ->
                //    (t, a) :> obj
                //| PropertyGetIdentity (t, p, a) ->
                //    (t, p, a) :> obj
                //| ValueIdentity (v, t) ->
                //    (v, t) :> obj
                //| CoerceIdentity (a, t) ->
                //    (a, t) :> obj
                //| NewUnionCaseIdentity (u, a) ->
                //    (u, a) :> obj
                //| ArgumentIdentity (n) ->
                //    n :> obj
            Comparer.Default.Compare (comparisonObj this, comparisonObj other)

    interface IComparable with
        member this.CompareTo other =
            (this :> IComparable<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>).CompareTo (other :?> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)

type Identity<'Identifier, 'Value, 'Type> = Identity<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type Identity<'Identifier, 'Value> = Identity<'Identifier, 'Value, Type>

type Identity<'Identifier> = Identity<'Identifier, obj>

type Identity = Identity<obj>