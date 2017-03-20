namespace Divination

open System

[<AutoOpen>]
module IDivinableExtensions =
    type IDivinable<'T, 'Identifier> with
        member this.Divine (scope : DivinationScope<'Identifier>, diviner : IDiviner<'Identifier>) =
            diviner.Divine<'T> (scope, this)

        member this.Divine (scope : DivinationScope<'Identifier>) =
            this.Divine (scope, FSharpDiviner<'Identifier>.Current)

        member this.Divine () =
            this.Divine DivinationScope.empty