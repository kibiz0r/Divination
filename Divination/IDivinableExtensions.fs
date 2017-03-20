namespace Divination

open System

[<AutoOpen>]
module IDivinableExtensions =
    type IDivinable<'T, 'Identifier> with
        member this.Divine (scope : IdentificationScope<'Identifier>, diviner : IDiviner<'Identifier>) =
            (diviner :> obj :?> IDiviner<'Identifier>).Divine<'T> (scope :> obj :?> IdentificationScope<'Identifier>, this :> obj :?> IDivinable<'T, 'Identifier>)

        member this.Divine (scope : IdentificationScope<'Identifier>) =
            this.Divine (scope, FSharpDiviner<'Identifier>.Current :> obj :?> _)

        member this.Divine () =
            this.Divine (IdentificationScope.empty ())