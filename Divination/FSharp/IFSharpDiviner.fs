namespace Divination.FSharp

open System
open Divination

type IFSharpDiviner =
    inherit IDiviner<FSharpExpr, IFSharpDiviningContext>
