namespace Divination.FSharp

open System

[<AutoOpen>]
module TopLevelOperations =
    let divine = FSharpDivineBuilder (FSharpDiviner (), FSharpExalter ())