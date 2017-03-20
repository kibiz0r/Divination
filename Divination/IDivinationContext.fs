namespace Divination

open System

type IDivinationContext<'Identifier> =
    abstract member Return : Identity<'Identifier>
        -> ContextualIdentity<'Identifier>

    abstract member Let : Identity<'Identifier>
        * Identity<'Identifier>
        -> IDivinationContext<'Identifier>
