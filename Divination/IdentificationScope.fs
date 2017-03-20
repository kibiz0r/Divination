namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IdentificationScope<'Identifier> = {
    Bindings : Map<Identity<'Identifier>, Identity<'Identifier>>
}
//with
//    interface IIdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
//        member this.Bindings = this.Bindings

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module IdentificationScope =
    let empty () : IdentificationScope<'Identifier> =
        { Bindings = Map.empty }

    let add
        (source : Identity<'Identifier>)
        (bound : Identity<'Identifier>)
        (scope : IdentificationScope<'Identifier>)
        : IdentificationScope<'Identifier> =
        { Bindings = Map.add source bound scope.Bindings }

    let tryFind
        (source : Identity<'Identifier>)
        (scope : IdentificationScope<'Identifier>)
        : Identity<'Identifier> option =
        Map.tryFind source scope.Bindings

    let merge
        (overridingScope : IdentificationScope<'Identifier>)
        (originalScope : IdentificationScope<'Identifier>)
        : IdentificationScope<'Identifier> =
        overridingScope