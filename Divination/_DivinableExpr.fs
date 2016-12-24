namespace Divination

open System

module ``DivinableExpr is dead`` =
    let x = 5

//type DivinableExpr<'T> = {
//    Raw : IDivinableExpr
//} with
//    interface IDivinableExpr<'T>

//module DivinableExpr =
//    let cast (raw : IDivinableExpr) : IDivinableExpr<'T> =
//        let d : DivinableExpr<'T> = { Raw = raw }
//        d :> IDivinableExpr<'T>

//type DivinableExpr<'T> (raw : DivinableExpr) =
//    member this.Raw = raw

//    interface IDivinable<'T>

//open System
//open System.Collections.Generic
//open System.Reflection
//open FSharp.Reflection
//open FSharp.Quotations
//open FSharp.Quotations.Patterns
//open FSharp.Quotations.Evaluator

//[<AbstractClass>]
//type DivinableExpr () =
//    abstract member ToExpr : unit -> Expr

//    member this.EvaluateUntyped () =
//        this.ToExpr () |> QuotationEvaluator.EvaluateUntyped

//    static member FromExpr (expr : Expr) : DivinableExpr =
//        let objExpr obj =
//            match obj with
//            | Some o -> Some (DivinableExpr.FromExpr o)
//            | None -> None
//        match expr with
//        | AddressOf (target) ->
//            let target' = DivinableExpr.FromExpr target
//            DivinableExpr.AddressOf (target')
//        | AddressSet (target, value) ->
//            let target' = DivinableExpr.FromExpr target
//            let value' = DivinableExpr.FromExpr value
//            DivinableExpr.AddressSet (target', value')
//        | Application (functionExpr, argument) ->
//            let functionExpr' = DivinableExpr.FromExpr functionExpr
//            let argument' = DivinableExpr.FromExpr argument
//            DivinableExpr.Application (functionExpr', argument')
//        | Call (obj, methodInfo, arguments) ->
//            let arguments' = arguments |> List.map (fun a -> DivinableExpr.FromExpr a)
//            DivinableExpr.Call (objExpr obj, methodInfo, arguments')
//        | Coerce (source, target) ->
//            let source' = DivinableExpr.FromExpr source
//            DivinableExpr.Coerce (source', target)
//        | DefaultValue (expressionType) ->
//            DivinableExpr.DefaultValue (expressionType)
//        | FieldGet (obj, fieldInfo) ->
//            DivinableExpr.FieldGet (objExpr obj, fieldInfo)
//        | FieldSet (obj, fieldInfo, value) ->
//            let value' = DivinableExpr.FromExpr value
//            DivinableExpr.FieldSet (objExpr obj, fieldInfo, value')
//        | ForIntegerRangeLoop (loopVariable, start, endExpr, body) ->
//            let loopVariable' = DivinableVar.FromVar loopVariable
//            let start' = DivinableExpr.FromExpr start
//            let endExpr' = DivinableExpr.FromExpr endExpr
//            let body' = DivinableExpr.FromExpr body
//            DivinableExpr.ForIntegerRangeLoop (loopVariable', start', endExpr', body')
//        | IfThenElse (guard, thenExpr, elseExpr) ->
//            let guard' = DivinableExpr.FromExpr guard
//            let thenExpr' = DivinableExpr.FromExpr thenExpr
//            let elseExpr' = DivinableExpr.FromExpr elseExpr
//            DivinableExpr.IfThenElse (guard', thenExpr', elseExpr')
//        | Lambda (parameter, body) ->
//            let parameter' = DivinableVar.FromVar parameter
//            let body' = DivinableExpr.FromExpr body
//            DivinableExpr.Lambda (parameter', body')
//        | Let (letVariable, letExpr, body) ->
//            let letVariable' = DivinableVar.FromVar letVariable
//            let letExpr' = DivinableExpr.FromExpr letExpr
//            let body' = DivinableExpr.FromExpr body
//            DivinableExpr.Let (letVariable', letExpr', body')
//        | LetRecursive (bindings, body) ->
//            let bindings' = bindings |> List.map (fun (v, e) -> (DivinableVar.FromVar v, DivinableExpr.FromExpr e))
//            let body' = DivinableExpr.FromExpr body
//            DivinableExpr.LetRecusirve (bindings', body')
//        | NewArray (elementType, elements) ->
//            let elements' = elements |> List.map (fun e -> DivinableExpr.FromExpr e)
//            DivinableExpr.NewArray (elementType, elements')
//        | NewDelegate (delegateType, parameters, body) ->
//            let parameters' = parameters |> List.map (fun p -> DivinableVar.FromVar p)
//            let body' = DivinableExpr.FromExpr body
//            DivinableExpr.NewDelegate (delegateType, parameters', body')
//        | NewObject (constructorInfo, arguments) ->
//            let arguments' = arguments |> List.map (fun a -> DivinableExpr.FromExpr a)
//            DivinableExpr.NewObject (constructorInfo, arguments')
//        | NewRecord (recordType, elements) ->
//            let elements' = elements |> List.map (fun e -> DivinableExpr.FromExpr e)
//            DivinableExpr.NewRecord (recordType, elements')
//        | NewTuple (elements) ->
//            let elements' = elements |> List.map (fun e -> DivinableExpr.FromExpr e)
//            DivinableExpr.NewTuple (elements')
//        | NewUnionCase (unionCase, arguments) ->
//            let arguments' = arguments |> List.map (fun a -> DivinableExpr.FromExpr a)
//            DivinableExpr.NewUnionCase (unionCase, arguments')
//        | PropertyGet (obj, property, indexerArgs) ->
//            let indexerArgs' = indexerArgs |> List.map (fun i -> DivinableExpr.FromExpr i)
//            DivinableExpr.PropertyGet (objExpr obj, property, indexerArgs')
//        | PropertySet (obj, property, indexerArgs, value) ->
//            let indexerArgs' = indexerArgs |> List.map (fun i -> DivinableExpr.FromExpr i)
//            let value' = DivinableExpr.FromExpr value
//            DivinableExpr.PropertySet (objExpr obj, property, value', indexerArgs')
//        | QuoteRaw (inner) ->
//            let inner' = DivinableExpr.FromExpr inner
//            DivinableExpr.QuoteRaw (inner')
//        | QuoteTyped (inner) ->
//            let inner' = DivinableExpr.FromExpr inner
//            DivinableExpr.QuoteTyped (inner')
//        | Sequential (first, second) ->
//            let first' = DivinableExpr.FromExpr first
//            let second' = DivinableExpr.FromExpr second
//            DivinableExpr.Sequential (first', second')
//        | TryFinally (body, compensation) ->
//            let body' = DivinableExpr.FromExpr body
//            let compensation' = DivinableExpr.FromExpr compensation
//            DivinableExpr.TryFinally (body', compensation')
//        | TryWith (body, filterVar, filterBody, catchVar, catchBody) ->
//            let body' = DivinableExpr.FromExpr body
//            let filterVar' = DivinableVar.FromVar filterVar
//            let filterBody' = DivinableExpr.FromExpr filterBody
//            let catchVar' = DivinableVar.FromVar catchVar
//            let catchBody' = DivinableExpr.FromExpr catchBody
//            DivinableExpr.TryWith (body', filterVar', filterBody', catchVar', catchBody')
//        | TupleGet (tuple, index) ->
//            let tuple' = DivinableExpr.FromExpr tuple
//            DivinableExpr.TupleGet (tuple', index)
//        | TypeTest (source, target) ->
//            let source' = DivinableExpr.FromExpr source
//            DivinableExpr.TypeTest (source', target)
//        | UnionCaseTest (source, unionCase) ->
//            let source' = DivinableExpr.FromExpr source
//            DivinableExpr.UnionCaseTest (source', unionCase)
//        | Value (value, expressionType) ->
//            DivinableExpr.Value (value, expressionType)
//        | ValueWithName (value, expressionType, name) ->
//            DivinableExpr.ValueWithName (value, expressionType, name)
//        | Var (variable) ->
//            let variable' = DivinableVar.FromVar variable
//            DivinableExpr.Var (variable')
//        | VarSet (variable, value) ->
//            let variable' = DivinableVar.FromVar variable
//            let value' = DivinableExpr.FromExpr value
//            DivinableExpr.VarSet (variable', value')
//        | WhileLoop (guard, body) ->
//            let guard' = DivinableExpr.FromExpr guard
//            let body' = DivinableExpr.FromExpr body
//            DivinableExpr.WhileLoop (guard', body')
//        | WithValue (value, expressionType, definition) ->
//            let definition' = DivinableExpr.FromExpr definition
//            DivinableExpr.WithValue (value, expressionType, definition')
//        | e ->
//            raise (Exception (sprintf "Unknown expression type %A" e))

//    static member AddressOf (target) =
//        DivinableExprAddressOf (target) :> DivinableExpr

//    static member AddressSet (target, value) =
//        DivinableExprAddressSet (target, value) :> DivinableExpr

//    static member Application (functionExpr, argument) =
//        DivinableExprApplication (functionExpr, argument) :> DivinableExpr

//    static member Applications (functionExpr, arguments) =
//        DivinableExprApplications (functionExpr, arguments) :> DivinableExpr

//    static member Call (obj, methodInfo, arguments) =
//        DivinableExprCall (obj, methodInfo, arguments) :> DivinableExpr

//    static member Cast<'T> (source) =
//        DivinableExpr<'T> (source)

//    static member Coerce (source, target) =
//        DivinableExprCoerce (source, target) :> DivinableExpr

//    static member DefaultValue (expressionType) =
//        DivinableExprDefaultValue (expressionType) :> DivinableExpr

//    static member Deserialize (qualifyingType, spliceTypes, spliceExprs, bytes) =
//        DivinableExprDeserialize (qualifyingType, spliceTypes, spliceExprs, bytes) :> DivinableExpr

//    static member FieldGet (obj, fieldInfo) =
//        DivinableExprFieldGet (obj, fieldInfo) :> DivinableExpr

//    static member FieldSet (obj, fieldInfo, value) =
//        DivinableExprFieldSet (obj, fieldInfo, value) :> DivinableExpr

//    static member ForIntegerRangeLoop (loopVariable, start, endExpr, body) =
//        DivinableExprForIntegerRangeLoop (loopVariable, start, endExpr, body) :> DivinableExpr

//    static member IfThenElse (guard, thenExpr, elseExpr) =
//        DivinableExprIfThenElse (guard, thenExpr, elseExpr) :> DivinableExpr

//    static member Lambda (parameter, body) =
//        DivinableExprLambda (parameter, body) :> DivinableExpr

//    static member Let (letVariable, letExpr, body) =
//        DivinableExprLet (letVariable, letExpr, body) :> DivinableExpr

//    static member LetRecusirve (bindings, body) =
//        DivinableExprLetRecursive (bindings, body) :> DivinableExpr

//    static member NewArray (elementType, elements) =
//        DivinableExprNewArray (elementType, elements) :> DivinableExpr

//    static member NewDelegate (delegateType, parameters, body) =
//        DivinableExprNewDelegate (delegateType, parameters, body) :> DivinableExpr

//    static member NewObject (constructorInfo, arguments) =
//        DivinableExprNewObject (constructorInfo, arguments) :> DivinableExpr

//    static member NewRecord (recordType, elements) =
//        DivinableExprNewRecord (recordType, elements) :> DivinableExpr

//    static member NewTuple (elements) =
//        DivinableExprNewTuple (elements) :> DivinableExpr

//    static member NewUnionCase (unionCase, arguments) =
//        DivinableExprNewUnionCase (unionCase, arguments) :> DivinableExpr

//    static member PropertyGet (obj, property, indexerArgs) =
//        DivinableExprPropertyGet (obj, property, indexerArgs) :> DivinableExpr

//    static member PropertySet (obj, property, value, indexerArgs) =
//        DivinableExprPropertySet (obj, property, value, indexerArgs) :> DivinableExpr

//    static member QuoteRaw (inner) =
//        DivinableExprQuoteRaw (inner) :> DivinableExpr

//    static member QuoteTyped (inner) =
//        DivinableExprQuoteTyped (inner) :> DivinableExpr

//    static member Sequential (first, second) =
//        DivinableExprSequential (first, second) :> DivinableExpr

//    static member TryFinally (body, compensation) =
//        DivinableExprTryFinally (body, compensation) :> DivinableExpr

//    static member TryWith (body, filterVar, filterBody, catchVar, catchBody) =
//        DivinableExprTryWith (body, filterVar, filterBody, catchVar, catchBody) :> DivinableExpr

//    static member TupleGet (tuple, index) =
//        DivinableExprTupleGet (tuple, index) :> DivinableExpr

//    static member TypeTest (source, target) =
//        DivinableExprTypeTest (source, target) :> DivinableExpr

//    static member UnionCaseTest (source, unionCase) =
//        DivinableExprUnionCaseTest (source, unionCase) :> DivinableExpr

//    static member Value (value, expressionType) =
//        DivinableExprValue (value, expressionType) :> DivinableExpr

//    static member ValueWithName (value, expressionType, name) =
//        DivinableExprValueWithName (value, expressionType, name) :> DivinableExpr

//    static member Var (variable) =
//        DivinableExprVar (variable) :> DivinableExpr

//    static member VarSet (variable, value) =
//        DivinableExprVarSet (variable, value) :> DivinableExpr

//    static member WhileLoop (guard, body) =
//        DivinableExprWhileLoop (guard, body) :> DivinableExpr

//    static member WithValue (value, expressionType, definition) =
//        DivinableExprWithValue (value, expressionType, definition) :> DivinableExpr

//and DivinableVar (var) =
//    static let globals = Dictionary<string * Type, DivinableVar> ()

//    member this.ToVar () = var
//        //match isMutable with
//        //| Some isMutable' ->
//        //    Var (name, typ, isMutable')
//        //| None ->
//        //    Var (name, typ)

//    static member FromVar (var) =
//        DivinableVar (var)
//        //DivinableVar (var.Name, var.Type, var.IsMutable)

//    //static member Global (name, typ) =
//    //    if name = null then
//    //        raise (ArgumentNullException "name")
//    //    if typ = null then
//    //        raise (ArgumentNullException "typ")

//    //    let dictionary = globals
//    //    lock dictionary (fun () ->
//    //        let key = (name, typ)
//    //        match dictionary.TryGetValue key with
//    //        | true, value -> value
//    //        | _ ->
//    //            let var = DivinableVar (name, typ, None)
//    //            dictionary.[(name, typ)] <- var
//    //            var
//    //    )

//and internal DivinableExprAddressOf (target : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let target' = target.ToExpr ()
//        Expr.AddressOf (target')

//and internal DivinableExprAddressSet (target : DivinableExpr, value : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let target' = target.ToExpr ()
//        let value' = value.ToExpr ()
//        Expr.AddressSet (target', value')

//and internal DivinableExprApplication (functionExpr : DivinableExpr, argument : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let functionExpr' = functionExpr.ToExpr ()
//        let argument' = argument.ToExpr ()
//        Expr.Application (functionExpr', argument')

//and internal DivinableExprApplications (functionExpr : DivinableExpr, arguments : DivinableExpr list list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let functionExpr' = functionExpr.ToExpr ()
//        let arguments' = arguments |> List.map (fun a's -> a's |> List.map (fun a -> a.ToExpr ()))
//        Expr.Applications (functionExpr', arguments')

//and internal DivinableExprCall (obj : DivinableExpr option, methodInfo : MethodInfo, arguments : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let arguments' = arguments |> List.map (fun a -> a.ToExpr ())
//        match obj with
//        | Some o -> 
//            Expr.Call (o.ToExpr (), methodInfo, arguments')
//        | None ->
//            Expr.Call (methodInfo, arguments')

//and internal DivinableExprCoerce (source : DivinableExpr, target : Type) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let source' = source.ToExpr ()
//        Expr.Coerce (source', target)

//and internal DivinableExprDefaultValue (expressionType : Type) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        Expr.DefaultValue (expressionType)

//and internal DivinableExprDeserialize (qualifyingType : Type, spliceTypes : Type list, spliceExprs : DivinableExpr list, bytes : byte []) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let spliceExprs' = spliceExprs |> List.map (fun s -> s.ToExpr ())
//        Expr.Deserialize (qualifyingType, spliceTypes, spliceExprs', bytes)

//and internal DivinableExprFieldGet (obj : DivinableExpr option, fieldInfo : FieldInfo) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        match obj with
//        | Some o -> Expr.FieldGet (o.ToExpr (), fieldInfo)
//        | None -> Expr.FieldGet (fieldInfo)

//and internal DivinableExprFieldSet (obj : DivinableExpr option, fieldInfo : FieldInfo, value : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let value' = value.ToExpr ()
//        match obj with
//        | Some o -> Expr.FieldSet (o.ToExpr (), fieldInfo, value')
//        | None -> Expr.FieldSet (fieldInfo, value')

//and internal DivinableExprForIntegerRangeLoop (loopVariable : DivinableVar, start : DivinableExpr, endExpr : DivinableExpr, body : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let loopVariable' = loopVariable.ToVar ()
//        let start' = start.ToExpr ()
//        let endExpr' = endExpr.ToExpr ()
//        let body' = body.ToExpr ()
//        Expr.ForIntegerRangeLoop (loopVariable', start', endExpr', body')

//and internal DivinableExprIfThenElse (guard : DivinableExpr, thenExpr : DivinableExpr, elseExpr : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let guard' = guard.ToExpr ()
//        let thenExpr' = thenExpr.ToExpr ()
//        let elseExpr' = elseExpr.ToExpr ()
//        Expr.IfThenElse (guard', thenExpr', elseExpr')

//and internal DivinableExprLambda (parameter : DivinableVar, body : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let parameter' = parameter.ToVar ()
//        let body' = body.ToExpr ()
//        Expr.Lambda (parameter', body')

//and internal DivinableExprLet (letVariable : DivinableVar, letExpr : DivinableExpr, body : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let letVariable' = letVariable.ToVar ()
//        let letExpr' = letExpr.ToExpr ()
//        let body' = body.ToExpr ()
//        Expr.Let (letVariable', letExpr', body')

//and internal DivinableExprLetRecursive (bindings : (DivinableVar * DivinableExpr) list, body : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let bindings' = bindings |> List.map (fun (v, e) -> (v.ToVar (), e.ToExpr ()))
//        let body' = body.ToExpr ()
//        Expr.LetRecursive (bindings', body')

//and internal DivinableExprNewArray (elementType : Type, elements : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let elements' = elements |> List.map (fun e -> e.ToExpr ())
//        Expr.NewArray (elementType, elements')

//and internal DivinableExprNewDelegate (delegateType : Type, parameters : DivinableVar list, body : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let parameters' = parameters |> List.map (fun p -> p.ToVar ())
//        let body' = body.ToExpr ()
//        Expr.NewDelegate (delegateType, parameters', body')

//and internal DivinableExprNewObject (constructorInfo : ConstructorInfo, arguments : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let arguments' = arguments |> List.map (fun a -> a.ToExpr ())
//        Expr.NewObject (constructorInfo, arguments')

//and internal DivinableExprNewRecord (recordType : Type, elements : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let elements' = elements |> List.map (fun e -> e.ToExpr ())
//        Expr.NewRecord (recordType, elements')

//and internal DivinableExprNewTuple (elements : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let elements' = elements |> List.map (fun e -> e.ToExpr ())
//        Expr.NewTuple (elements')

//and internal DivinableExprNewUnionCase (unionCase : UnionCaseInfo, arguments : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let arguments' = arguments |> List.map (fun a -> a.ToExpr ())
//        Expr.NewUnionCase (unionCase, arguments')

//and internal DivinableExprPropertyGet (obj : DivinableExpr option, property : PropertyInfo, indexerArgs : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let indexerArgs' = indexerArgs |> List.map (fun i -> i.ToExpr ())
//        match obj with
//        | Some o -> Expr.PropertyGet (o.ToExpr (), property, indexerArgs')
//        | None -> Expr.PropertyGet (property, indexerArgs')

//and internal DivinableExprPropertySet (obj : DivinableExpr option, property : PropertyInfo, value : DivinableExpr, indexerArgs : DivinableExpr list) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let value' = value.ToExpr ()
//        let indexerArgs' = indexerArgs |> List.map (fun i -> i.ToExpr ())
//        match obj with
//        | Some o -> Expr.PropertySet (o.ToExpr (), property, value', indexerArgs')
//        | None -> Expr.PropertySet (property, value', indexerArgs')

//and internal DivinableExprQuoteRaw (inner : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let inner' = inner.ToExpr ()
//        Expr.QuoteRaw (inner')

//and internal DivinableExprQuoteTyped (inner : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let inner' = inner.ToExpr ()
//        Expr.QuoteTyped (inner')

//and internal DivinableExprSequential (first : DivinableExpr, second : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let first' = first.ToExpr ()
//        let second' = second.ToExpr ()
//        Expr.Sequential (first', second')

//and internal DivinableExprTryFinally (body : DivinableExpr, compensation : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let body' = body.ToExpr ()
//        let compensation' = compensation.ToExpr ()
//        Expr.TryFinally (body', compensation')

//and internal DivinableExprTryWith (body : DivinableExpr, filterVar : DivinableVar, filterBody : DivinableExpr, catchVar : DivinableVar, catchBody : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let body' = body.ToExpr ()
//        let filterVar' = filterVar.ToVar ()
//        let filterBody' = filterBody.ToExpr ()
//        let catchVar' = catchVar.ToVar ()
//        let catchBody' = catchBody.ToExpr ()
//        Expr.TryWith (body', filterVar', filterBody', catchVar', catchBody')

//and internal DivinableExprTupleGet (tuple : DivinableExpr, index : int) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let tuple' = tuple.ToExpr ()
//        Expr.TupleGet (tuple', index)

//and internal DivinableExprTypeTest (source : DivinableExpr, target : Type) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let source' = source.ToExpr ()
//        Expr.TypeTest (source', target)

//and internal DivinableExprUnionCaseTest (source : DivinableExpr, unionCase : UnionCaseInfo) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let source' = source.ToExpr ()
//        Expr.UnionCaseTest (source', unionCase)

//and internal DivinableExprValue (value : obj, expressionType : Type) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        Expr.Value (value, expressionType)

//and internal DivinableExprValueWithName (value : obj, expressionType : Type, name : string) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        Expr.ValueWithName (value, expressionType, name)

//and internal DivinableExprVar (variable : DivinableVar) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let variable' = variable.ToVar ()
//        Expr.Var (variable')

//and internal DivinableExprVarSet (variable : DivinableVar, value : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let variable' = variable.ToVar ()
//        let value' = value.ToExpr ()
//        Expr.VarSet (variable', value')

//and internal DivinableExprWhileLoop (guard : DivinableExpr, body : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let guard' = guard.ToExpr ()
//        let body' = body.ToExpr ()
//        Expr.WhileLoop (guard', body')

//and internal DivinableExprWithValue (value : obj, expressionType : Type, definition : DivinableExpr) =
//    inherit DivinableExpr ()

//    override this.ToExpr () =
//        let definition' = definition.ToExpr ()
//        Expr.WithValue (value, expressionType, definition')

//and DivinableExpr<'T> (raw : DivinableExpr) =
//    inherit DivinableExpr ()

//    member this.Raw = raw

//    override this.ToExpr () =
//        raw.ToExpr ()

//    member this.ToExprTyped () =
//        Expr.Cast<'T> (this.ToExpr ())

//    member this.Evaluate () =
//        this.ToExprTyped () |> QuotationEvaluator.Evaluate