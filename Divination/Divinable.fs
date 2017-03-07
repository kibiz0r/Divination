namespace Divination

open System
open FSharp.Quotations

type Divinable<'T> (body : IDiviner * IDivinationBinding -> Divined<'T>) =
    interface IDivinable<'T> with
        member this.Divine (diviner : IDiviner, binding : IDivinationBinding) : Divined<'T> =
            body (diviner, binding)