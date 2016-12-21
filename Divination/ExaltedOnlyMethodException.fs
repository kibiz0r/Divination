namespace Divination

open System

type ExaltedOnlyMethodException () =
    inherit Exception ("This is a exalted marker method; it should not be invoked directly.")