namespace Divination

open System
open System.Reflection
open FSharp.Reflection

//type DivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> () =
//    abstract member Identity : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//        -> IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//    default this.Identity (identity) =
//        {
//            new DivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> () with
//                member this.Resolve (diviner) =
//                    identity
//        } :> _

//    abstract member Let : IDivinableBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//        * IDivinableBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//        * IDivinableBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//        -> IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//    default this.Let (var, argument, body) =
//        {
//            new DivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> () with
//                member this.Resolve (diviner) =
//                    let var' = (var.Contextualize this).Resolve diviner
//                    let argument' = (argument.Contextualize this).Resolve diviner
//                    let body' = (body.Contextualize this).Resolve diviner
//                    LetIdentity (var', argument', body')
//        } :> _

//    abstract member Bind : IDivinableBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//        * (IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> -> IDivinableBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>)
//        -> IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//    default this.Bind (argument, body) =
//        this :> _

//    abstract member Return : IDivinableBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//        -> IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//    default this.Return (argument) =
//        this :> _

//    abstract member Resolve : IDivinerBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, IIdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//        -> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
//    default this.Resolve (diviner) =
//        invalidOp "No Identity provided to DivinationContext"

//    interface IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> with
//        member this.Identity (identity) =
//            this.Identity identity

//        member this.Let (var, argument, body) =
//            this.Let (var, argument, body)

//        member this.Bind (argument, body) =
//            this.Bind (argument, body)

//        member this.Return (argument) =
//            this.Return argument

//        member this.Resolve (diviner) =
//            this.Resolve diviner

// A DivinationContext supports a divinable's ability to generate its *contextual* identity, which may be an identity
// that is relative to parent contexts, or an absolute/canonical identity, and it carries with it any resolved
// identities that were evaluated in obtaining its contextual identity.
// 
// The context allows the divinable to:
// - Identify other divinables
//   - Within this context, or a derived one, or new one
//   - Those identities are also contextual
// - Resolve arbitrary identities
//   - Again, within any context
//   - Resolutions are stored, by identity, so that subsequent resolutions do not evaluate the same identity twice
// - Translate identities that belong to other contexts into identities with respect to this context
//   - This does not necessarily make them canonical identities
// 
// The context does not:
// - Give direct access to the diviner
type DivinationContext<'Identifier> (diviner : IDiviner<'Identifier>, scope : DivinationScope<'Identifier>) =
    interface IDivinationContext<'Identifier> with
        member this.Return identity : ContextualIdentity<'Identifier> =
            { Scope = scope; Identity = identity }

        member this.Let (var : Identity<'Identifier>, argument : Identity<'Identifier>) : IDivinationContext<'Identifier> =
            let scope = { scope with IdentificationScope = IdentificationScope.add var argument scope.IdentificationScope }
            DivinationContext (diviner, scope) :> _

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module DivinationContext =
    let return' identity (context : IDivinationContext<_>) =
        context.Return identity

    let let' var argument (context : IDivinationContext<_>) =
        context.Let (var, argument)