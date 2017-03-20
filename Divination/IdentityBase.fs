namespace Divination

open System
open System.Collections
open System.Reflection
open FSharp.Reflection

// The concept of Identity is key to the entirety of Divination
[<CustomEquality; CustomComparison>]
type IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    | IdentifierBase of
        'Identifier
    | CallIdentityBase of
        IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'MethodInfo
        * IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | NewObjectIdentityBase of
        'ConstructorInfo
        * IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | PropertyGetIdentityBase of
        IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'PropertyInfo
        * IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | ValueIdentityBase of
        'Value
        * 'Type
    | CoerceIdentityBase of
        IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * 'Type
    | NewUnionCaseIdentityBase of
        'UnionCaseInfo
        * IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
    | VarIdentityBase of
        string
    | LetIdentityBase of
        IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
with
    override this.ToString () =
        sprintf "%A" this

    override this.Equals other =
        match other with
        | :? IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> as o ->
            (this :> IEquatable<IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>).Equals o
        | _ -> false

    override this.GetHashCode () =
        match this with
        | IdentifierBase i ->
            ("Identifier", i :> obj).GetHashCode ()
        | CallIdentityBase (t, m, a) ->
            ("CallIdentity", t :> obj, m :> obj, a).GetHashCode ()
        | NewObjectIdentityBase (c, a) ->
            ("NewObjectIdentity", c :> obj, a).GetHashCode ()
        | PropertyGetIdentityBase (t, p, a) ->
            ("PropertyGetIdentity", t :> obj, p :> obj, a).GetHashCode ()
        | ValueIdentityBase (v, t) ->
            ("ValueIdentity", v :> obj, t :> obj).GetHashCode ()
        | CoerceIdentityBase (a, t) ->
            ("CoerceIdentity", a :> obj, t :> obj).GetHashCode ()
        | NewUnionCaseIdentityBase (u, a) ->
            ("NewUnionCaseIdentity", u :> obj, a).GetHashCode ()
        | VarIdentityBase (n) ->
            ("VarIdentity", n :> obj).GetHashCode ()
        | LetIdentityBase (v, a, b) ->
            ("LetIdentity", v :> obj, a :> obj, b :> obj).GetHashCode ()

    interface IEquatable<IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>> with
        member this.Equals other =
            match this, other with
            | IdentifierBase i, IdentifierBase i2 ->
                (i :> obj).Equals i2
            | CallIdentityBase (t, m, a), CallIdentityBase (t2, m2, a2) ->
                (t :> obj, m :> obj, a).Equals ((t2 :> obj, m2 :> obj, a2))
            | NewObjectIdentityBase (c, a), NewObjectIdentityBase (c2, a2) ->
                (c :> obj, a).Equals ((c2 :> obj, a2))
            | PropertyGetIdentityBase (t, p, a), PropertyGetIdentityBase (t2, p2, a2) ->
                (t :> obj, p :> obj, a).Equals ((t2 :> obj, p2 :> obj, a2))
            | ValueIdentityBase (v, t), ValueIdentityBase (v2, t2) ->
                (v :> obj, t :> obj).Equals ((v2 :> obj, t2 :> obj))
            | CoerceIdentityBase (a, t), CoerceIdentityBase (a2, t2) ->
                (a :> obj, t :> obj).Equals ((a2 :> obj, t2 :> obj))
            | NewUnionCaseIdentityBase (u, a), NewUnionCaseIdentityBase (u2, a2) ->
                (u :> obj, a).Equals ((u2 :> obj, a2))
            | VarIdentityBase (n), VarIdentityBase (n2) ->
                (n :> obj).Equals n2
            | LetIdentityBase (v, a, b), LetIdentityBase (v2, a2, b2) ->
                (v :> obj, a :> obj, b :> obj).Equals ((v2 :> obj, a2 :> obj, b2 :> obj))
            | _ -> false

    interface IComparable<IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>> with
        member this.CompareTo other =
            Comparer.Default.Compare (this.GetHashCode (), other.GetHashCode ())

    interface IComparable with
        member this.CompareTo other =
            (this :> IComparable<IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>).CompareTo (other :?> IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)
