namespace Divination

open System

type UnwrapDivinable<'T> (wrappedDivinable : IDivinable<IDivinable<'T>>) =
    interface IDivinable<'T> with
        member this.Identify diviner =
            Diviner.identify diviner wrappedDivinable