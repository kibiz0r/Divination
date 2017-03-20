namespace Divination

open System

[<AutoOpen>]
module IDivinerExtensions =
    type IDiviner<'Identifier> with
        member this.Divine<'T> (scope : IdentificationScope<'Identifier>, divinable : IDivinable<'T, 'Identifier>) =
            let newContext = this.NewContext scope
            let contextualized = divinable.Contextualize newContext
            let identity = this.EvaluateContext contextualized
            let value = this.Resolve<'T> (scope, identity)
            DivinedValue (identity, value)

        member this.Divine (divinable : IDivinable<'T, 'Identifier>) =
            this.Divine (IdentificationScope.empty (), FSharpDiviner<'Identifier>.Current :> obj :?> _)
