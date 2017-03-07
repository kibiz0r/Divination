namespace Divination.Test

open System
open System.Reactive.Subjects
    
module Observable =
    let toConnectedList (observable : IConnectableObservable<_>) =
        let mutable values = []
        use __ = Observable.subscribe (fun value -> values <- List.append values [value]) observable
        values
