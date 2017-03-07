namespace Divination.Test

open System
open FSharp.Control.Reactive

type NoArgumentConstructorType () =
    static let constructed = Event<unit> ()

    do constructed.Trigger ()

    static member Constructed : IObservable<unit> = constructed.Publish :> IObservable<unit>