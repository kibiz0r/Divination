namespace Divination

open System
open FSharp.Quotations

type Divinable<'T> (identityFunc : IDiviner -> Identity) =
    interface IDivinable<'T> with
        member this.Divine (diviner : IDiviner, binding : IDivinationBinding) : Divined<'T> =
            let identity = identityFunc diviner
            match binding.TryGet identity with
            | Some entry -> 
                entry
            | None ->
                let value = diviner.Resolve identity
                DivinedValue (identity, value)