namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IFSharpDiviner<'Identifier> =
    inherit IDiviner<'Identifier>