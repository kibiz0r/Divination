namespace Divination

open System

type ExaltedOnlyPropertyException () =
    inherit Exception ("This is a exalted marker property; it should not be invoked directly.")
