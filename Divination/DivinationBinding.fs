namespace Divination

open System

type DivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> = {
    Entries : Map<Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>, Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>>
}
with
    interface IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> with
        member this.TryGet<'T> (local : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option =
            match Map.tryFind local this.Entries with
            | Some entry ->
                Some (Divined.cast entry)
            | None -> None

        member this.Set<'T> (local : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>, external : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>) : IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
            { Entries = Map.add local (Divined.cast external) this.Entries } :> IDivinationBinding<_, _, _, _, _, _>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module DivinationBinding =
    let empty () =
        { Entries = Map.empty } :> IDivinationBinding<_, _, _, _, _, _>