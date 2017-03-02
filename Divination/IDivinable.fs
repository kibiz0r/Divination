namespace Divination

open System
open System.Reflection

// A divinable has two components:
// 1: Its type, which is what it resolves to through use of a diviner
// 2: Its identity, which is expected to be immutable and serializable
//    Common identities include:
//    - Expr<'T>
type IDivinable<'T, 'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> =
    // The divinable is only given a diviner for computing its identity so that divinables can be identified in terms
    // of other divinables while remaining lazily-evaluated
    abstract member Identify : IDiviner<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo> -> Identity<'Identifier, 'Value, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo>

type IDivinable<'T, 'Identifier, 'Value> = IDivinable<'T, 'Identifier, 'Value, ConstructorInfo, MethodInfo, PropertyInfo>

type IDivinable<'T, 'Identifier> = IDivinable<'T, 'Identifier, obj>

type IDivinable<'T> = IDivinable<'T, obj>