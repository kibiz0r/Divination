namespace Divination

open System

[<AutoOpen>]
module TopLevelOperations =
    let divinable = DivinableBuilder (Exalter ())
    let divined = DivinedBuilder (Exalter (), Diviner (), DiviningContext ())