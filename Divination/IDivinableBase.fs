namespace Divination

open System
open System.Reflection
open FSharp.Reflection
    
type IDivinableBase<'Context, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    abstract member Contextualize : 'Context -> 'Context
