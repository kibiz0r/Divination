namespace Divination

open System
open System.Reflection

type Divined<'T, 'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    | DivinedValue of Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> * 'T
    | DivinedException of Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> * exn
with
    member this.Identity =
        match this with
        | DivinedValue (i, _) -> i
        | DivinedException (i, _) -> i

    member this.Value =
        match this with
        | DivinedValue (_, v) -> v
        | DivinedException (_, e) -> raise e

    member this.Exception =
        match this with
        | DivinedValue (_, _) -> null
        | DivinedException (_, e) -> e

type Divined<'T, 'Identifier, 'Value> = Divined<'T, 'Identifier, 'Value, ConstructorInfo, MethodInfo, PropertyInfo>

type Divined<'T, 'Identifier> = Divined<'T, 'Identifier, obj>

type Divined<'T> = Divined<'T, obj>