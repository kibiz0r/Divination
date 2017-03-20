namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type IdentityBase<'Identifier, 'Value, 'Type> = IdentityBase<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IdentityBase<'Identifier, 'Value> = IdentityBase<'Identifier, 'Value, Type>
type IdentityBase<'Identifier> = IdentityBase<'Identifier, obj>
type IdentityBase = IdentityBase<obj>

type Identity = Identity<obj>

type Identified<'T> = Identified<'T, obj>

type IIdentifiable<'T> = IIdentifiable<'T, obj>

type DivinedBase<'Identifier, 'Value, 'Type> = DivinedBase<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type DivinedBase<'Identifier, 'Value> = DivinedBase<'Identifier, 'Value, Type>
type DivinedBase<'Identifier> = DivinedBase<'Identifier, obj>
type DivinedBase = DivinedBase<obj>

type Divined<'T> = Divined<'T, obj>

type DivinationContext = DivinationContext<obj>

type IdentificationScope = IdentificationScope<obj>

type IDivinerBase<'Context, 'Scope> = IDivinerBase<'Context, 'Scope, obj>
type IDivinerBase<'Context> = IDivinerBase<'Context, IdentificationScope>
type IDivinerBase = IDivinerBase<DivinationContext>

type IDiviner = IDiviner<obj>

type Diviner = Diviner<obj>

type IDivinableBase<'Context, 'Identifier, 'Value, 'Type> = IDivinableBase<'Context, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IDivinableBase<'Context, 'Identifier, 'Value> = IDivinableBase<'Context, 'Identifier, 'Value, Type>
type IDivinableBase<'Context, 'Identifier> = IDivinableBase<'Context, 'Identifier, obj>
type IDivinableBase<'Context> = IDivinableBase<'Context, obj>
type IDivinableBase = IDivinableBase<DivinationContext>

type IDivinable<'T> = IDivinable<'T, obj>

type DivinableBase<'Context, 'Identifier, 'Value, 'Type> = DivinableBase<'Context, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type DivinableBase<'Context, 'Identifier, 'Value> = DivinableBase<'Context, 'Identifier, 'Value, Type>
type DivinableBase<'Context, 'Identifier> = DivinableBase<'Context, 'Identifier, obj>
type DivinableBase<'Context> = DivinableBase<'Context, obj>
type DivinableBase = DivinableBase<DivinationContext>

type Divinable<'T> = Divinable<'T, obj>

type IExprDivinifierBase<'Context, 'Identifier, 'Value, 'Type> = IExprDivinifierBase<'Context, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IExprDivinifierBase<'Context, 'Identifier, 'Value> = IExprDivinifierBase<'Context, 'Identifier, 'Value, Type>
type IExprDivinifierBase<'Context, 'Identifier> = IExprDivinifierBase<'Context, 'Identifier, obj>
type IExprDivinifierBase<'Context> = IExprDivinifierBase<'Context, obj>
type IExprDivinifierBase = IExprDivinifierBase<DivinationContext>

type IExprDivinifier = IExprDivinifier<obj>

type ExprDivinifier = ExprDivinifier<obj>

type IExprIdentifier = IExprIdentifier<obj>

type ExprIdentifier = ExprIdentifier<obj>
