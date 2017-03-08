namespace Divination

open System
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator

type DivinableBuilder () =
    member this.Bind ([<ReflectedDefinition>] divinableExpr : Expr<IDivinable<'T>>, body : 'T -> IDivinable<'U>) : IDivinable<'U> =
        Divinable<'U> (fun binding ->
            let divinableIdentity = divinableExpr.ToIdentity ()
            let divinable : IDivinable<'T> =
                match divinableIdentity with
                | CallIdentity (this', methodInfo, arguments) ->
                    let arguments' = List.map (fun argument -> binding.Diviner.Resolve<obj> (binding, argument)) arguments
                    let parameterInfos = methodInfo.GetParameters ()
                    let bindingWithArguments = Seq.zip arguments' parameterInfos |> Seq.fold (fun (b : IDivinationBinding) (a, p) -> b.Set<obj> ((VarIdentity (p.Name, a.Value, p.ParameterType)), a)) binding
                    binding.Diviner.ResolveValue (bindingWithArguments, divinableIdentity)
                | _ ->
                    binding.Diviner.ResolveValue (binding, divinableIdentity)
            let divinedValue = binding.Diviner.ResolveValue (binding, divinable.Identify binding)
            let bodyDivinable = body divinedValue
            let bodyIdentity = bodyDivinable.Identify binding
            bodyIdentity
        ) :> IDivinable<'U>

    member this.Return ([<ReflectedDefinition>] returnExpr : Expr<'T>) : IDivinable<'T> =
        Divinable<'T> (fun binding -> returnExpr.ToIdentity ()) :> IDivinable<'T>