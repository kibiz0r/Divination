namespace Divination

open System
open System.Reflection

type IIdentificationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    abstract member Get : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> ->
        Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>

    abstract member Set : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>
        * Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>
        -> IIdentificationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>

type IIdentificationBinding<'Identifier, 'Value, 'Type> = IIdentificationBinding<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo>

type IIdentificationBinding<'Identifier, 'Value> = IIdentificationBinding<'Identifier, 'Value, Type>

type IIdentificationBinding<'Identifier> = IIdentificationBinding<'Identifier, obj>

type IIdentificationBinding = IIdentificationBinding<obj>