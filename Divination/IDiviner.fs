namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IDiviner<'Identifier> =
    abstract member Resolve : DivinationScope<'Identifier>
        * Identity<'Identifier>
        -> obj

    abstract member Resolve<'T> : DivinationScope<'Identifier>
        * Identity<'Identifier>
        -> 'T

    abstract member NewContext : DivinationScope<'Identifier>
        -> IDivinationContext<'Identifier>

    abstract member Canonicalize : DivinationScope<'Identifier>
        * ContextualIdentity<'Identifier>
        -> DivinationScope<'Identifier> * Identity<'Identifier>
