namespace Divination.FSharp

open System
open Divination

type FSharpDiviningContext (vars : Map<_, obj>) as this =
    new () = FSharpDiviningContext Map.empty

    member this.GetVar (var : FSharpVar) : obj =
        vars.[var]

    member this.SetVar (var : FSharpVar, value : obj) : FSharpDiviningContext =
        FSharpDiviningContext (vars |> Map.add var value)

    interface IFSharpDiviningContext with
        member i.SetVar (var : FSharpVar, value : obj) =
            this.SetVar (var, value) :> IFSharpDiviningContext

    interface IDiviningContext<FSharpVar> with
        member i.GetVar (var : FSharpVar) : obj =
            this.GetVar var

        member i.SetVar (var : FSharpVar, value : obj) : IDiviningContext<FSharpVar> =
            this.SetVar (var, value) :> IDiviningContext<FSharpVar>

    interface IDiviningContext with
        member i.GetVar (var : obj) : obj =
            this.GetVar (var :?> FSharpVar)

        member i.SetVar (var : obj, value : obj) : IDiviningContext =
            this.SetVar (var :?> FSharpVar, value) :> IDiviningContext