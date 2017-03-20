namespace Divination

open System

type DivinedBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    | DivinedValueBase of IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> * obj
    | DivinedExceptionBase of IdentityBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> * exn
with
    member this.Identity =
        match this with
        | DivinedValueBase (i, _) -> i
        | DivinedExceptionBase (i, _) -> i

    member this.Value =
        match this with
        | DivinedValueBase (_, v) -> v
        | DivinedExceptionBase (_, e) -> raise e

    member this.Exception =
        match this with
        | DivinedValueBase (_, _) -> null
        | DivinedExceptionBase (_, e) -> e

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module DivinedBase =
    let cast (divined : DivinedBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>) : DivinedBase<'Identifier2, 'Value2, 'Type2, 'ConstructorInfo2, 'MethodInfo2, 'PropertyInfo2, 'UnionCaseInfo2> =
        match divined with
        | DivinedValueBase (identity, value) ->
            DivinedValueBase (identity :> obj :?> IdentityBase<'Identifier2, 'Value2, 'Type2, 'ConstructorInfo2, 'MethodInfo2, 'PropertyInfo2, 'UnionCaseInfo2>, value)
        | DivinedExceptionBase (identity, exception') ->
            DivinedExceptionBase (identity :> obj :?> IdentityBase<'Identifier2, 'Value2, 'Type2, 'ConstructorInfo2, 'MethodInfo2, 'PropertyInfo2, 'UnionCaseInfo2>, exception')