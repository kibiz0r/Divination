namespace Divination

open System

module ``DiviningContext is dead`` =
    let x = 5
//type DiviningContext (vars : Map<_, _>) =
//    new () = DiviningContext Map.empty

//    interface IDiviningContext with
//        member this.GetVar (name) =
//            vars.[name]

//        member this.SetVar (name, value) =
//            DiviningContext (vars |> Map.add name value) :> IDiviningContext