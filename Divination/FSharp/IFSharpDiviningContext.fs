namespace Divination.FSharp

open System
open Divination
    
type IFSharpDiviningContext =
    inherit IDiviningContext<FSharpVar>
    abstract member SetVar : FSharpVar * obj -> IFSharpDiviningContext