namespace Divination

open System

type Divinable<'T> (value : unit -> 'T) =
    member this.Value =
        value ()
