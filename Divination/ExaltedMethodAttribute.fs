namespace Divination

open System

type ExaltedMethodAttribute (exaltedMethodName : string) =
    inherit Attribute ()

    member this.ExaltedMethodName = exaltedMethodName