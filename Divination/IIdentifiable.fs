namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IIdentifiable<'T, 'Identifier> =
    abstract member Identify : IdentificationScope<'Identifier>
        -> Identified<'T, 'Identifier>
