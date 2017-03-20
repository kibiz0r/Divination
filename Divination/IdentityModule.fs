namespace Divination

open System

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Identity =
    let value (value : 'T) : Identity<'Identifier> =
        Identity<'Identifier>.ValueIdentity (value :> obj, typeof<'T>)

    let cast (identity : Identity<'Identifier>) : Identity<'Identifier2> =
        identity :> obj :?> Identity<'Identifier2>