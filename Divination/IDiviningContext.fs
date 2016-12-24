namespace Divination

open System

type IDiviningContext =
    abstract member GetVar : obj -> obj
    abstract member SetVar : obj * obj -> IDiviningContext

type IDiviningContext<'Var when 'Var :> IDivineVar> =
    inherit IDiviningContext
    abstract member GetVar : 'Var -> obj
    abstract member SetVar : 'Var * obj -> IDiviningContext<'Var>
