namespace Divination

open System
open System.Reflection

// A divinable has two components:
// 1: Its type, which is what it resolves to through use of a diviner
// 2: Its identity, which is expected to be immutable and serializable
//    Common identities include:
//    - Expr<'T>
type IDivinable<'T, 'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    // It doesn't seem like this signature is quite right anymore...
    // It's more like it needs to accept a "Binding" or something, which can translate Identities across stack frames
    // so that they maintain coherence.
    abstract member Identify : IDiviner<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> -> Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>

type IDivinable<'T, 'Identifier, 'Value> = IDivinable<'T, 'Identifier, 'Value, ConstructorInfo, MethodInfo, PropertyInfo>

type IDivinable<'T, 'Identifier> = IDivinable<'T, 'Identifier, obj>

type IDivinable<'T> = IDivinable<'T, obj>