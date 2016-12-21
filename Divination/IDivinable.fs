namespace Divination

open System

type IDivinable =
    interface
    end

and IDivinable<'T> =
    inherit IDivinable

type Divined<'T> = {
    Source : IDivinable
    [<ExaltedProperty("Source")>]
    Value : 'T
}

//and IDivined<'T> =
//    inherit IDivined
//    abstract member Source : IDivinable<'T>
//    [<ExaltedProperty("Source")>]
//    abstract member Value : 'T

and IDiviner =
    interface
    end