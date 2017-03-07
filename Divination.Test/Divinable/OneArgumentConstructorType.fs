namespace Divination.Test

open System

type OneArgumentConstructorType (str : string) =
    static let constructed = Event<unit> ()

    do constructed.Trigger ()

    static member Constructed : IObservable<unit> = constructed.Publish :> IObservable<unit>

    member val Str = str