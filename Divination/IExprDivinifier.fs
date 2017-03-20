namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open FSharp.Quotations
open FSharp.Quotations.Patterns

type IExprDivinifier<'Identifier> =
    inherit IExprDivinifierBase<DivinationContext<'Identifier>, 'Identifier, obj, Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
    abstract member ToDivinable<'T> : Expr<'T> -> IDivinable<'T, 'Identifier>
