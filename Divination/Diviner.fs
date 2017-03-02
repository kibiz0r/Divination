namespace Divination

open System
open System.Reflection

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    let identify (divinable : IDivinable<'T, _, _, _, _, _>) (diviner : IDiviner<_, _, _, _, _>) : Identity<_, _, _, _, _> =
        divinable.Identify diviner

    let rec resolve (identity : Identity<_, _, _, _, _>) (diviner : IDiviner<_, _, _, _, _>) : 'T =
        match identity with
        | Identifier identifier ->
            diviner.Identifier identifier
        | CallIdentity (this, methodInfo, arguments) ->
            diviner.Call (this, methodInfo, arguments)
        | ConstructorIdentity (constructorInfo, arguments) ->
            diviner.Constructor (constructorInfo, arguments)
        | PropertyGetIdentity (this, propertydInfo, arguments) ->
            diviner.PropertyGet (this, propertydInfo, arguments)

    let value (divinable : IDivinable<'T, _, _, _, _, _>) (diviner : IDiviner<_, _, _, _, _>) : 'T =
        resolve (identify divinable diviner) diviner

    let divine (diviner : IDiviner<_, _, _, _, _>) (divinable : IDivinable<'T, _, _, _, _, _>) : Divined<'T, _, _, _, _, _> =
        let identity = identify divinable diviner
        try
            let value = resolve identity diviner
            DivinedValue (identity, value)
        with
            | e -> DivinedException (identity, e)

type FSharpDiviner () =
    interface IDiviner

    abstract member Identifier<'T> : obj -> 'T
    default this.Identifier (identifier : obj) : 'T =
        raise (NotImplementedException ())

    abstract member CallIdentity<'T> : Identity option * MethodInfo * Identity list -> 'T
    default this.CallIdentity<'T> (this' : Identity option, methodInfo : MethodInfo, arguments : Identity list) : 'T =
        let this'' =
            match this' with
            | Some t -> Diviner.resolve this t
            | None -> null
        let arguments' = List.map (Diviner.resolve this) arguments |> List.toArray
        methodInfo.Invoke (this'', arguments') :?> 'T

    abstract member ConstructorIdentity<'T> : ConstructorInfo * Identity list -> 'T
    default this.ConstructorIdentity<'T> (constructorInfo : ConstructorInfo, arguments : Identity list) : 'T =
        let arguments' = List.map (Diviner.resolve this) arguments |> List.toArray
        constructorInfo.Invoke (arguments') :?> 'T