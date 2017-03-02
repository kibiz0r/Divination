namespace Divination

open System
open System.Reflection

type IDiviner<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    abstract member Identifier<'T> : 'Identifier -> 'T
    abstract member Call<'T> : Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option
        * 'MethodInfo
        * Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
        -> 'T
    abstract member Constructor<'T> : 'ConstructorInfo
        * Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
        -> 'T
    abstract member PropertyGet<'T> : Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option
        * 'PropertyInfo
        * Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> list
        -> 'T

type IDiviner<'Identifier, 'Value> = IDiviner<'Identifier, 'Value, ConstructorInfo, MethodInfo, PropertyInfo>

type IDiviner<'Identifier> = IDiviner<'Identifier, obj>

type IDiviner = IDiviner<obj>