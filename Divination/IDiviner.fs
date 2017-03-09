namespace Divination

open System
open System.Reflection
open FSharp.Reflection

// A Diviner can attempt to resolve any given Identity plus Type to an instance of that Type.
type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    abstract member Identifier<'T> : 'Identifier
        -> 'T

    abstract member Call<'T> : Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'MethodInfo
        * Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> 'T
    
    abstract member Constructor<'T> : 'ConstructorInfo
        * Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> 'T
    
    abstract member PropertyGet<'T> : Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> option
        * 'PropertyInfo
        * Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> 'T

    abstract member Value<'T> : 'Value
        * 'Type
        -> 'T

    abstract member Coerce<'T> : Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        * 'Type
        -> 'T

    abstract member NewUnionCase<'T> : 'UnionCaseInfo
        * Divined<obj, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> list
        -> 'T

    abstract member Var<'T> : string * 'Type -> 'T

type IDiviner<'Identifier, 'Value, 'Type> = IDiviner<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IDiviner<'Identifier, 'Value> = IDiviner<'Identifier, 'Value, Type>

type IDiviner<'Identifier> = IDiviner<'Identifier, obj>

type IDiviner = IDiviner<obj>