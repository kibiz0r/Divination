namespace Divination.Test

open System
open System.Reactive.Subjects
open FSharp.Quotations
open FSharp.Quotations.Patterns
    
module Observable =
    let toConnectedList (observable : IConnectableObservable<_>) =
        let mutable values = []
        use __ = Observable.subscribe (fun value -> values <- List.append values [value]) observable
        values

[<AutoOpen>]
module Utils =
    type Expr with
        member this.ToMethodInfo () =
            match this with
            | Call (_, methodInfo, _) -> methodInfo
            | _ -> invalidOp (sprintf "Expr is not a Call: %A" this)