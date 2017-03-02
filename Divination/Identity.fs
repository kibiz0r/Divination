namespace Divination

open System
open System.Collections
open System.Reflection

[<CustomEquality; CustomComparison>]
type Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    | Identifier of
        'Identifier
    | CallIdentity of
        Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option
        * 'MethodInfo
        * Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
    | ConstructorIdentity of
        'ConstructorInfo
        * Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
    | PropertyGetIdentity of
        Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option
        * 'PropertyInfo
        * Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
with
    override this.Equals other =
        match other with
        | :? Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> as o ->
            (this :> IEquatable<Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>>).Equals o
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

    interface IEquatable<Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>> with
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
            | _ -> false

    interface IComparable<Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>> with
        member this.CompareTo other =
            let comparisonObj identity =
                match identity with
                | Identifier i ->
                    i :> obj
                | CallIdentity (t, m, a) ->
                    (t, m, a) :> obj
                | ConstructorIdentity (t, a) ->
                    (t, a) :> obj
                | PropertyGetIdentity (t, p, a) ->
                    (t, p, a) :> obj
            Comparer.Default.Compare (comparisonObj this, comparisonObj other)

    interface IComparable with
        member this.CompareTo other =
            (this :> IComparable<Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>>).CompareTo (other :?> Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>)

type Identity<'Identifier, 'Value> = Identity<'Identifier, 'Value, ConstructorInfo, MethodInfo, PropertyInfo>

type Identity<'Identifier> = Identity<'Identifier, obj>

type Identity = Identity<obj>