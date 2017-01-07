namespace Divination

open System

type BindDivinable<'T, 'U> (body : 'T -> IDivinable<'U>, argument : IDivinable<'T>) =
    interface IDivinable<'U> with
        member this.Identify (diviner : IDiviner) =
            let argument' = Diviner.identifyAndDivineValue diviner argument
            body argument' |> Diviner.identify diviner