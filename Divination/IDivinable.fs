namespace Divination

open System
open System.Reflection
open FSharp.Reflection

// A Divinable is a yet-to-be-materialized object, that is destined to be a particular type, and has a particular
// Identity, but may need to phrase that Identity in terms of other Identities that are not known to it at the time
// when the Divinable is first created. Once that Identity is fully formed, that plus the designation of its type is
// enough for a Diviner to resolve it to an instance.
type IDivinable<'T, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo> =
    inherit IDivinableBase<IDivinationContext<'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>, 'Identifier, 'Value, 'Type, 'ConstructorInfo, 'MethodInfo, 'PropertyInfo, 'UnionCaseInfo>