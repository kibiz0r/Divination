namespace Divination

open System

type DivinationScope<'Identifier> = {
    IdentificationScope : IdentificationScope<'Identifier>
    ResolutionMap : Map<Identity<'Identifier>, obj>
}

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module DivinationScope =
    let empty =
        { IdentificationScope = IdentificationScope.empty; ResolutionMap = Map.empty }