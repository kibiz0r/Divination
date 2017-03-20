namespace Divination

open System

type ContextualIdentity<'Identifier> = {
    Scope : DivinationScope<'Identifier>
    Identity : Identity<'Identifier>
}
