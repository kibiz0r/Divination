namespace Divination

open System
open System.Collections

[<CustomEquality; CustomComparison>]
type Identity =
    | Identifier of obj
    | CallIdentity of Identity option * obj * Identity list
    | ConstructorIdentity of obj * Identity list
    | PropertyGetIdentity of Identity option * obj * Identity list
with
    override this.Equals other =
        match other with
        | :? Identity as o ->
            (this :> IEquatable<Identity>).Equals o
        | _ -> false

    override this.GetHashCode () =
        match this with
        | Identifier i ->
            i.GetHashCode ()
        | CallIdentity (t, m, a) ->
            (t, m, a).GetHashCode ()
        | ConstructorIdentity (t, a) ->
            (t, a).GetHashCode ()
        | PropertyGetIdentity (t, p, a) ->
            (t, p, a).GetHashCode ()

    interface IEquatable<Identity> with
        member this.Equals other =
            match this, other with
            | Identifier i, Identifier i2 ->
                i.Equals i2
            | CallIdentity (t, m, a), CallIdentity (t2, m2, a2) ->
                (t, m, a).Equals ((t2, m2, a2))
            | ConstructorIdentity (c, a), ConstructorIdentity (c2, a2) ->
                (c, a).Equals ((c2, a2))
            | PropertyGetIdentity (t, p, a), PropertyGetIdentity (t2, p2, a2) ->
                (t, p, a).Equals ((t2, p2, a2))
            | _ -> false

    interface IComparable<Identity> with
        member this.CompareTo other =
            let comparisonObj identity =
                match identity with
                | Identifier i ->
                    i
                | CallIdentity (t, m, a) ->
                    (t, m, a) :> obj
                | ConstructorIdentity (t, a) ->
                    (t, a) :> obj
                | PropertyGetIdentity (t, p, a) ->
                    (t, p, a) :> obj
            Comparer.Default.Compare (comparisonObj this, comparisonObj other)

    interface IComparable with
        member this.CompareTo other =
            (this :> IComparable<Identity>).CompareTo (other :?> Identity)