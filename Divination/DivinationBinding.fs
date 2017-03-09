namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type DivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> = {
    Entries : Map<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>>
}

type DivinationBinding<'Identifier, 'Value, 'Type> = DivinationBinding<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type DivinationBinding<'Identifier, 'Value> = DivinationBinding<'Identifier, 'Value, Type>

type DivinationBinding<'Identifier> = DivinationBinding<'Identifier, obj>

type DivinationBinding = DivinationBinding<obj>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module DivinationBinding =
    let empty () =
        { Entries = Map.empty }

    let add (identity : Identity<_, _, _, _, _, _, _>) (divined : Divined<_, _, _, _, _, _, _, _>) (binding : DivinationBinding<_, _, _, _, _, _, _>) : DivinationBinding<_, _, _, _, _, _, _> =
        { Entries = Map.add identity (Divined.cast divined) binding.Entries }

    let tryFind (identity : Identity<_, _, _, _, _, _, _>) (binding : DivinationBinding<_, _, _, _, _, _, _>) : Divined<_, _, _, _, _, _, _, _> option =
        match Map.tryFind identity binding.Entries with
        | Some entry ->
            Some (Divined.cast entry)
        | None -> None

    let merge (overridingBinding : DivinationBinding<_, _, _, _, _, _, _>) (originalBinding : DivinationBinding<_, _, _, _, _, _, _>) : DivinationBinding<_, _, _, _, _, _, _> =
        overridingBinding