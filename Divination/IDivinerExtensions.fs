namespace Divination

open System

[<AutoOpen>]
module IDivinerExtensions =
    type IDiviner<'Identifier> with
        member this.Divine<'T> (scope : DivinationScope<'Identifier>, divinable : IDivinable<'T, 'Identifier>) =
            let newContext = this.NewContext scope
            let contextualIdentity = divinable.Contextualize newContext
            let canonicalScope, canonicalIdentity = this.Canonicalize (scope, contextualIdentity)
            let value = this.Resolve<'T> (canonicalScope, canonicalIdentity)
            DivinedValue (canonicalIdentity, value)

        member this.Divine (divinable : IDivinable<'T, 'Identifier>) =
            this.Divine (DivinationScope.empty, FSharpDiviner<'Identifier>.Current :> obj :?> _)
