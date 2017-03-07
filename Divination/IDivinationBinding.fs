namespace Divination

open System
open System.Reflection

type IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    abstract member TryGet<'T> : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>
        -> Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> option

    abstract member Set<'T> : Identity<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>
        * Divined<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>
        -> IDivinationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>

type IDivinationBinding<'Identifier, 'Value, 'Type> = IDivinationBinding<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo>

type IDivinationBinding<'Identifier, 'Value> = IDivinationBinding<'Identifier, 'Value, Type>

type IDivinationBinding<'Identifier> = IDivinationBinding<'Identifier, obj>

type IDivinationBinding = IDivinationBinding<obj>