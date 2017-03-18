namespace Divination

open System
open System.Reflection
open FSharp.Reflection

type Identity<'Identifier, 'Value, 'Type> = Identity<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type Identity<'Identifier, 'Value> = Identity<'Identifier, 'Value, Type>
type Identity<'Identifier> = Identity<'Identifier, obj>
type Identity = Identity<obj>

type Identified<'T, 'Identifier, 'Value, 'Type> = Identified<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type Identified<'T, 'Identifier, 'Value> = Identified<'T, 'Identifier, 'Value, Type>
type Identified<'T, 'Identifier> = Identified<'T, 'Identifier, obj>
type Identified<'T> = Identified<'T, obj>

type IIdentifiable<'T, 'Identifier, 'Value, 'Type> = IIdentifiable<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IIdentifiable<'T, 'Identifier, 'Value> = IIdentifiable<'T, 'Identifier, 'Value, Type>
type IIdentifiable<'T, 'Identifier> = IIdentifiable<'T, 'Identifier, obj>
type IIdentifiable<'T> = IIdentifiable<'T, obj>

type Divined<'T, 'Identifier, 'Value, 'Type> = Divined<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type Divined<'T, 'Identifier, 'Value> = Divined<'T, 'Identifier, 'Value, Type>
type Divined<'T, 'Identifier> = Divined<'T, 'Identifier, obj>
type Divined<'T> = Divined<'T, obj>

type IDivinationContext<'Identifier, 'Value, 'Type> = IDivinationContext<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IDivinationContext<'Identifier, 'Value> = IDivinationContext<'Identifier, 'Value, Type>
type IDivinationContext<'Identifier> = IDivinationContext<'Identifier, obj>
type IDivinationContext = IDivinationContext<obj>

type DivinationContext<'Identifier, 'Value, 'Type> = DivinationContext<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type DivinationContext<'Identifier, 'Value> = DivinationContext<'Identifier, 'Value, Type>
type DivinationContext<'Identifier> = DivinationContext<'Identifier, obj>
type DivinationContext = DivinationContext<obj>

type IDivinerBase<'Context, 'Scope, 'Identifier, 'Value, 'Type> = IDivinerBase<'Context, 'Scope, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IDivinerBase<'Context, 'Scope, 'Identifier, 'Value> = IDivinerBase<'Context, 'Scope, 'Identifier, 'Value, Type>
type IDivinerBase<'Context, 'Scope, 'Identifier> = IDivinerBase<'Context, 'Scope, 'Identifier, obj>
type IDivinerBase<'Context, 'Scope> = IDivinerBase<'Context, 'Scope, obj>

type IDiviner<'Scope, 'Identifier, 'Value, 'Type> = IDiviner<'Scope, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IDiviner<'Scope, 'Identifier, 'Value> = IDiviner<'Scope, 'Identifier, 'Value, Type>
type IDiviner<'Scope, 'Identifier> = IDiviner<'Scope, 'Identifier, obj>
type IDiviner<'Scope> = IDiviner<'Scope, obj>

type Diviner<'Identifier, 'Value, 'Type> = Diviner<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type Diviner<'Identifier, 'Value> = Diviner<'Identifier, 'Value, Type>
type Diviner<'Identifier> = Diviner<'Identifier, obj>

type IDivinableBase<'Context, 'Identifier, 'Value, 'Type> = IDivinableBase<'Context, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IDivinableBase<'Context, 'Identifier, 'Value> = IDivinableBase<'Context, 'Identifier, 'Value, Type>
type IDivinableBase<'Context, 'Identifier> = IDivinableBase<'Context, 'Identifier, obj>
type IDivinableBase<'Context> = IDivinableBase<'Context, obj>

type IDivinable<'T, 'Identifier, 'Value, 'Type> = IDivinable<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IDivinable<'T, 'Identifier, 'Value> = IDivinable<'T, 'Identifier, 'Value, Type>
type IDivinable<'T, 'Identifier> = IDivinable<'T, 'Identifier, obj>
type IDivinable<'T> = IDivinable<'T, obj>

type DivinableBase<'Context, 'Identifier, 'Value, 'Type> = DivinableBase<'Context, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type DivinableBase<'Context, 'Identifier, 'Value> = DivinableBase<'Context, 'Identifier, 'Value, Type>
type DivinableBase<'Context, 'Identifier> = DivinableBase<'Context, 'Identifier, obj>
type DivinableBase<'Context> = DivinableBase<'Context, obj>
type DivinableBase = DivinableBase<IDivinationContext>

type Divinable<'T, 'Identifier, 'Value, 'Type> = Divinable<'T, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type Divinable<'T, 'Identifier, 'Value> = Divinable<'T, 'Identifier, 'Value, Type>
type Divinable<'T, 'Identifier> = Divinable<'T, 'Identifier, obj>
type Divinable<'T> = Divinable<'T, obj>

type IExprDivinifierBase<'Context, 'Identifier, 'Value, 'Type> = IExprDivinifierBase<'Context, 'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IExprDivinifierBase<'Context, 'Identifier, 'Value> = IExprDivinifierBase<'Context, 'Identifier, 'Value, Type>
type IExprDivinifierBase<'Context, 'Identifier> = IExprDivinifierBase<'Context, 'Identifier, obj>
type IExprDivinifierBase<'Context> = IExprDivinifierBase<'Context, obj>
type IExprDivinifierBase = IExprDivinifierBase<IDivinationContext>

type IExprDivinifier<'Identifier, 'Value, 'Type> = IExprDivinifier<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IExprDivinifier<'Identifier, 'Value> = IExprDivinifier<'Identifier, 'Value, Type>
type IExprDivinifier<'Identifier> = IExprDivinifier<'Identifier, obj>
type IExprDivinifier = IExprDivinifier<obj>

type ExprDivinifier<'Identifier, 'Value, 'Type> = ExprDivinifier<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type ExprDivinifier<'Identifier, 'Value> = ExprDivinifier<'Identifier, 'Value, Type>
type ExprDivinifier<'Identifier> = ExprDivinifier<'Identifier, obj>
type ExprDivinifier = ExprDivinifier<obj>

type IExprIdentifier<'Identifier, 'Value, 'Type> = IExprIdentifier<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IExprIdentifier<'Identifier, 'Value> = IExprIdentifier<'Identifier, 'Value, Type>
type IExprIdentifier<'Identifier> = IExprIdentifier<'Identifier, obj>
type IExprIdentifier = IExprIdentifier<obj>

type ExprIdentifier<'Identifier, 'Value, 'Type> = ExprIdentifier<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type ExprIdentifier<'Identifier, 'Value> = ExprIdentifier<'Identifier, 'Value, Type>
type ExprIdentifier<'Identifier> = ExprIdentifier<'Identifier, obj>
type ExprIdentifier = ExprIdentifier<obj>

type IIdentificationScope<'Identifier, 'Value, 'Type> = IIdentificationScope<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IIdentificationScope<'Identifier, 'Value> = IIdentificationScope<'Identifier, 'Value, Type>
type IIdentificationScope<'Identifier> = IIdentificationScope<'Identifier, obj>
type IIdentificationScope = IIdentificationScope<obj>

type IdentificationScope<'Identifier, 'Value, 'Type> = IdentificationScope<'Identifier, 'Value, 'Type, ConstructorInfo, MethodInfo, PropertyInfo, UnionCaseInfo>
type IdentificationScope<'Identifier, 'Value> = IdentificationScope<'Identifier, 'Value, Type>
type IdentificationScope<'Identifier> = IdentificationScope<'Identifier, obj>
type IdentificationScope = IdentificationScope<obj>