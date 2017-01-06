namespace Divination

open System

type UnwrapDivinable<'T> (divinable : IDivinable<IDivinable<'T>>) =
    interface IDivinable<'T> with
        member this.Identity diviner =
            (Diviner.divine diviner divinable).Identity diviner