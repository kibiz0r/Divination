namespace Divination

open System
open System.Reflection
open FSharp.Reflection
    
type IDivinableBase<'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    // The scope helps the Divinable structure its Identity in terms of other Identities that are known to
    // the caller but not known to the Divinable.
    abstract member Contextualize : 'Context -> 'Context

type IDivinableBase<'Context, 'Identifier, 'Value, 'Type> = IDivinableBase<'Context, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IDivinableBase<'Context, 'Identifier, 'Value> = IDivinableBase<'Context, 'Identifier, 'Value, Type>

type IDivinableBase<'Context, 'Identifier> = IDivinableBase<'Context, 'Identifier, obj>

type IDivinableBase<'Context> = IDivinableBase<'Context, obj>