namespace Divination

open System
open System.Collections
open System.Reflection
open FSharp.Reflection

// The concept of Identity is key to the entirety of Divination
[<CustomEquality; CustomComparison>]
type Identity<'Identifier> =
    | Identifier of
        'Identifier
    | CallIdentity of
        Identity<'Identifier> option
        * MethodInfo
        * Identity<'Identifier> list
    | NewObjectIdentity of
        ConstructorInfo
        * Identity<'Identifier> list
    | PropertyGetIdentity of
        Identity<'Identifier> option
        * PropertyInfo
        * Identity<'Identifier> list
    | ValueIdentity of
        obj
        * Type
    | CoerceIdentity of
        Identity<'Identifier>
        * Type
    | NewUnionCaseIdentity of
        UnionCaseInfo
        * Identity<'Identifier> list
    | VarIdentity of
        string
    | LetIdentity of
        Identity<'Identifier>
        * Identity<'Identifier>
        * Identity<'Identifier>
with
    override this.ToString () =
        sprintf "%A" this

    override this.Equals other =
        match other with
        | :? Identity<'Identifier> as o ->
            (this :> IEquatable<Identity<'Identifier>>).Equals o
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
            ("ValueIdentity", v, t :> obj).GetHashCode ()
        | CoerceIdentity (a, t) ->
            ("CoerceIdentity", a :> obj, t :> obj).GetHashCode ()
        | NewUnionCaseIdentity (u, a) ->
            ("NewUnionCaseIdentity", u :> obj, a).GetHashCode ()
        | VarIdentity (n) ->
            ("VarIdentity", n :> obj).GetHashCode ()
        | LetIdentity (v, a, b) ->
            ("LetIdentity", v :> obj, a :> obj, b :> obj).GetHashCode ()

    interface IEquatable<Identity<'Identifier>> with
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
                (v, t :> obj).Equals ((v2, t2 :> obj))
            | CoerceIdentity (a, t), CoerceIdentity (a2, t2) ->
                (a :> obj, t :> obj).Equals ((a2 :> obj, t2 :> obj))
            | NewUnionCaseIdentity (u, a), NewUnionCaseIdentity (u2, a2) ->
                (u :> obj, a).Equals ((u2 :> obj, a2))
            | VarIdentity (n), VarIdentity (n2) ->
                (n :> obj).Equals n2
            | LetIdentity (v, a, b), LetIdentity (v2, a2, b2) ->
                (v :> obj, a :> obj, b :> obj).Equals ((v2 :> obj, a2 :> obj, b2 :> obj))
            | _ -> false


    interface IComparable<Identity<'Identifier>> with
        member this.CompareTo other =
            Comparer.Default.Compare (this.GetHashCode (), other.GetHashCode ())

    interface IComparable with
        member this.CompareTo other =
            (this :> IComparable<Identity<'Identifier>>).CompareTo (other :?> Identity<'Identifier>)
