namespace Divination

open System

// A divinable has two components:
// 1: Its type, which is what it resolves to through use of a diviner
// 2: Its identity, which is expected to be immutable and serializable
//    Common identities include:
//    - Expr<'T>
[<Serializable>]
type IDivinable<'T> =
    // The divinable is only given a diviner for computing its identity so that divinables can be identified in terms
    // of other divinables while remaining lazily-evaluated
    abstract member Identity : IDiviner -> obj