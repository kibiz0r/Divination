namespace Divination

open System
open System.Reflection
open FSharp.Interop.Dynamic
open FSharp.Quotations
open FSharp.Quotations.Evaluator

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Diviner =
    let identify (diviner : IDiviner) (divinable : IDivinable<'T>) : Identity =
        divinable.Identify diviner

    let rec resolve (diviner : IDiviner) (identity : Identity) : 'T =
        match identity with
        | Identifier identifier ->
            diviner?Identifier identifier
        | CallIdentity (this, methodObj, arguments) ->
            diviner?CallIdentity (this, methodObj, arguments)
        | ConstructorIdentity (ctorObj, arguments) ->
            diviner?ConstructorIdentity (ctorObj, arguments)
        | PropertyGetIdentity (this, propertydObj, arguments) ->
            diviner?PropertyGetIdentity (this, propertydObj, arguments)

    let value (diviner : IDiviner) (divinable : IDivinable<'T>) : 'T =
        identify diviner divinable |> resolve diviner

    let divine (diviner : IDiviner) (divinable : IDivinable<'T>) : Divined<'T> =
        let identity = identify diviner divinable
        try
            let value = resolve diviner identity
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