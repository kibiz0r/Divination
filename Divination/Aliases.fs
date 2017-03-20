namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type Identity = Identity<obj>

type Identified<'T> = Identified<'T, obj>
type Identified = Identified<obj>

type IIdentifiable<'T> = IIdentifiable<'T, obj>
type IIdentifiable = IIdentifiable<obj>

type Divined<'T> = Divined<'T, obj>
type Divined = Divined<obj>

type DivinationContext = DivinationContext<obj>

type IdentificationScope = IdentificationScope<obj>

type IDiviner = IDiviner<obj>

type Diviner = Diviner<obj>

type IDivinable<'T> = IDivinable<'T, obj>
type IDivinable = IDivinable<obj>

type Divinable<'T> = Divinable<'T, obj>
type Divinable = Divinable<obj>

type IExprDivinifier = IExprDivinifier<obj>

type ExprDivinifier = ExprDivinifier<obj>

type IExprIdentifier = IExprIdentifier<obj>

type ExprIdentifier = ExprIdentifier<obj>
