namespace Divination

open System

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module DivinationContext =
    let identify (divinable : IDivinable<_, _>) (withIdentity : Identity<_> -> DivinationContext<_>) (context : DivinationContext<_>) : DivinationContext<_> =
        { Scope = context.Scope; Operation = DivinationIdentify (divinable, withIdentity) }

    let return' (identity : Identity<'Identifier>) (context : DivinationContext<'Identifier>) : DivinationContext<'Identifier> =
        { Scope = context.Scope; Operation = DivinationReturn identity }

    let returnFrom (divinable : IDivinable<_, _>) (context : DivinationContext<'Identifier>) : DivinationContext<'Identifier> =
        context |> identify divinable (fun identity ->
            return' identity context
        )

    let let' (var : IDivinable<_, _>) (argument : IDivinable<_, _>) (body : DivinationContext<_> -> DivinationContext<_>) (context : DivinationContext<_>) : DivinationContext<_> =
        context |> identify argument (fun argument' ->
            context |> identify var (fun var' ->
                {
                    Scope = IdentificationScope.add var' argument' context.Scope
                    Operation = DivinationReturn (ValueIdentity ("nope" :> obj, typeof<string>))
                } |> body
            )
        )

    let rec evaluate (diviner : IDiviner<'Identifier>) (context : DivinationContext<'Identifier>) : Identity<'Identifier> =
        match context.Operation with
        | DivinationReturn identity ->
            identity
        | DivinationIdentify (divinable, withIdentity) ->
            let newContext = diviner.NewContext context.Scope
            let contextualized = divinable.Contextualize newContext
            let identity = diviner.EvaluateContext contextualized
            evaluate diviner (withIdentity identity)