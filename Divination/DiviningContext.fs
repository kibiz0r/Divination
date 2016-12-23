namespace Divination

open System

type DiviningContext (vars : Map<_, _>) =
    new () = DiviningContext Map.empty

    interface IDiviningContext with
        member this.GetVar (name) =
            vars.[name]

        member this.SetVar (name, value) =
            DiviningContext (vars |> Map.add name value) :> IDiviningContext