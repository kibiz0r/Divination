namespace Divination

open System

type Divined<'T> = {
    Source : IDivineExpr
    Value : 'T
}