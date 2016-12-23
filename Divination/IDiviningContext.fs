namespace Divination

open System

type IDiviningContext =
    abstract member GetVar : string -> obj
    abstract member SetVar : string * obj -> IDiviningContext
