namespace Divination

open System
open System.Reflection

// A Diviner can attempt to resolve any given Identity plus Type to an instance of that Type.
type IDiviner<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    abstract member Identifier<'T> : 'Identifier -> 'T

    abstract member Call<'T> : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option
        * 'MethodInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
        -> 'T
    
    abstract member Constructor<'T> : 'ConstructorInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
        -> 'T
    
    abstract member PropertyGet<'T> : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option
        * 'PropertyInfo
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
        -> 'T

    abstract member Value<'T> : 'Value * 'Type -> 'T

type IDiviner<'Identifier, 'Value, 'Type> = IDiviner<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo>

type IDiviner<'Identifier, 'Value> = IDiviner<'Identifier, 'Value, Type>

type IDiviner<'Identifier> = IDiviner<'Identifier, obj>

type IDiviner = IDiviner<obj>