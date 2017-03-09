namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> = {
    Bindings : Map<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>
}

type IdentificationScope<'Identifier, 'Value, 'Type> = IdentificationScope<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IdentificationScope<'Identifier, 'Value> = IdentificationScope<'Identifier, 'Value, Type>

type IdentificationScope<'Identifier> = IdentificationScope<'Identifier, obj>

type IdentificationScope = IdentificationScope<obj>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module IdentificationScope =
    let empty () : IdentificationScope =
        { Bindings = Map.empty }

    let add (source : Identity<_, _, _, _, _, _, _>) (bound : Identity<_, _, _, _, _, _, _>) (scope : IdentificationScope<_, _, _, _, _, _, _>) : IdentificationScope<_, _, _, _, _, _, _> =
        { Bindings = Map.add source bound scope.Bindings }

    let tryFind (source : Identity<_, _, _, _, _, _, _>) (scope : IdentificationScope<_, _, _, _, _, _, _>) : Identity<_, _, _, _, _, _, _> option =
        Map.tryFind source scope.Bindings

    let merge (overridingScope : IdentificationScope<_, _, _, _, _, _, _>) (originalScope : IdentificationScope<_, _, _, _, _, _, _>) : IdentificationScope<_, _, _, _, _, _, _> =
        overridingScope