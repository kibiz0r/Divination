namespace Divination

open System
open FSharp.Quotations

module Divinable =
    let identify (diviner : IDiviner<_, _, _, _, _>) (divinable : IDivinable<'T, _, _, _, _, _>) : Identity<_, _, _, _, _> =
        divinable.Identify diviner