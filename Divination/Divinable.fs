namespace Divination

open System
open FSharp.Quotations

type Divinable<'T> (identify : IDivinationBinding -> Identity) =
    interface IDivinable<'T> with
        member this.Identify (binding : IDivinationBinding) : Identity =
            identify binding

[<AutoOpen>]
module Divinable =
    type IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
        member this.Divine (binding : IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
            let identity = this.Identify binding
            binding.Diviner.Resolve<'T> (binding, identity)