namespace Divination

open System

type DivinationContextualIdentity<'Identifier> = {
    Context : DivinationContext<'Identifier>
    Identity : Identity<'Identifier>
}
