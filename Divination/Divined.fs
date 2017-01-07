namespace Divination

open System

type Divined<'T> = {
    Identity : obj
    Value : 'T
}
