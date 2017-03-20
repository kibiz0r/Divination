namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IDiviner<'Identifier> =
    inherit IDivinerBase<DivinationContext<'Identifier>, IdentificationScope<'Identifier>, Identity<'Identifier>>

    abstract member Resolve<'T> : IdentificationScope<'Identifier>
        * Identity<'Identifier>
        -> 'T