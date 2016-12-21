namespace Divination

open System

type Diviner () =
    interface IDiviner

    member this.Divine (divinable : IDivinable) : obj =
        raise (NotImplementedException ())

    member this.Divine<'T> (divinable : IDivinable<'T>) : 'T =
        raise (NotImplementedException ())

    member this.Divine (divinable : DivinableValue) : obj =
        divinable.Value

    member this.Divine (value : DivinableUnion) : obj =
        raise (Exception "got union")

    //member this.Divine (value : DivinableValueUnionCaseType) : obj =
    //    raise (Exception "got union case type")