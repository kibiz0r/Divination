namespace Divination

open System
open FSharp.Quotations

// Diviner.divine does dynamic dispatch to find the best overload of Divine on the implementer of IDiviner
type IDiviner =
    interface
    end