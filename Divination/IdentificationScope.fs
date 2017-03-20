namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IdentificationScope<'Identifier> = {
    IdentityMap : Map<Identity<'Identifier>, Identity<'Identifier>>
}

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module IdentificationScope =
    let empty : IdentificationScope<'Identifier> =
        { IdentityMap = Map.empty }

    let add
        (source : Identity<'Identifier>)
        (bound : Identity<'Identifier>)
        (scope : IdentificationScope<'Identifier>)
        : IdentificationScope<'Identifier> =
        { IdentityMap = Map.add source bound scope.IdentityMap }

    let tryFind
        (source : Identity<'Identifier>)
        (scope : IdentificationScope<'Identifier>)
        : Identity<'Identifier> option =
        Map.tryFind source scope.IdentityMap

    let merge
        (overridingScope : IdentificationScope<'Identifier>)
        (originalScope : IdentificationScope<'Identifier>)
        : IdentificationScope<'Identifier> =
        overridingScope