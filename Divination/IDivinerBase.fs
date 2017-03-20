namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IDivinerBase<'Context, 'Scope, 'Identity> =
    abstract member Resolve : 'Scope
        * 'Identity
        -> obj

    abstract member NewContext : 'Scope
        -> 'Context

    abstract member EvaluateContext : 'Context
        -> 'Identity