namespace Divination

open System
open System.Reflection
open FSharp.Reflection
    
type IDivinableBase<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    // The scope helps the Divinable structure its Identity in terms of other Identities that are known to
    // the caller but not known to the Divinable.
    abstract member Identify : IdentificationScope<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        -> Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

type IDivinableBase<'Identifier, 'Value, 'Type> = IDivinableBase<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IDivinableBase<'Identifier, 'Value> = IDivinableBase<'Identifier, 'Value, Type>

type IDivinableBase<'Identifier> = IDivinableBase<'Identifier, obj>

type IDivinableBase = IDivinableBase<obj>