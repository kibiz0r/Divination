﻿namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IIdentifiable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    abstract member Identify : IIdentificationBinding<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>
        -> Identified<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>

type IIdentifiable<'T, 'Identifier, 'Value, 'Type> = IIdentifiable<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>

type IIdentifiable<'T, 'Identifier, 'Value> = IIdentifiable<'T, 'Identifier, 'Value, Type>

type IIdentifiable<'T, 'Identifier> = IIdentifiable<'T, 'Identifier, obj>

type IIdentifiable<'T> = IIdentifiable<'T, obj>