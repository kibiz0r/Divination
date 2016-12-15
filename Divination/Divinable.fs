namespace Divination

//open System
//open FSharp.Quotations
//open FSharp.Quotations.Evaluator
//open FSharp.Reflection

//module NewDivinable =
//    type IDiviner =
//        abstract member Type : string -> Type
//        abstract member Value : obj * Type -> obj
//        abstract member ValueWithName : string * Type -> obj
//        abstract member PropertyGet : obj * Type * string -> obj
//        abstract member Call : obj * Type * string * Type list * obj list -> obj
//        abstract member Coerce : obj * Type -> obj
//        abstract member NewUnionCase : Type * string * obj list -> obj

//    type DefaultDiviner () =
//        interface IDiviner with
//            member this.Type name =
//                Type.GetType name

//            member this.Value (value, valueType) =
//                if valueType.IsPrimitive then
//                    if valueType.IsAssignableFrom (value.GetType ()) then
//                        value
//                    else
//                        Convert.ChangeType (value, valueType)
//                else
//                    Activator.CreateInstance (valueType, [value])

//            member this.ValueWithName (valueName, valueType) =
//                obj ()

//            member this.PropertyGet (this', declaringType, propertyName) =
//                let propertyInfo = declaringType.GetProperty propertyName
//                propertyInfo.GetValue this'

//            member this.Call (this', declaringType, methodName, typeArguments, arguments) =
//                let methodInfo = declaringType.GetMethod methodName
//                if List.isEmpty typeArguments then
//                    methodInfo.Invoke (this', List.toArray arguments)
//                else
//                    let methodInfo' = methodInfo.MakeGenericMethod (List.toArray typeArguments)
//                    methodInfo'.Invoke (this', List.toArray arguments)

//            member this.Coerce (toCoerce, coercionType) =
//                Convert.ChangeType (toCoerce, coercionType)

//            member this.NewUnionCase (declaringType, unionCaseName, unionCases) =
//                let unionCaseInfos = FSharpType.GetUnionCases (declaringType, true)
//                let unionCaseInfo = unionCaseInfos |> Array.find (fun u -> u.Name = unionCaseName)
//                let constructor' = Reflection.FSharpValue.PreComputeUnionConstructor (unionCaseInfo, true)
//                unionCases |> List.toArray |> constructor'

//    type IDivinable =
//        abstract member Divine : IDiviner -> obj

//    type DivinableType (name : string) =
//        let simpleName =
//            let commaIndex = name.IndexOf ","
//            if commaIndex > 0 then
//                name.Substring (0, commaIndex)
//            else
//                name

//        override this.ToString () =
//            sprintf "Type (%A)" simpleName

//        interface IDivinable with
//            member this.Divine diviner =
//                diviner.Type name :> obj

//    type DivinableValue (value : obj, valueType : IDivinable) =
//        override this.ToString () =
//            sprintf "Value (%A, %A)" value valueType

//        interface IDivinable with
//            member this.Divine diviner =
//                let valueType' = valueType.Divine diviner :?> Type
//                diviner.Value (value, valueType')

//    type DivinableValueWithName (valueName : string, valueType : IDivinable) =
//        override this.ToString () =
//            sprintf "ValueWithName (%A, %A)" valueName valueType

//        interface IDivinable with
//            member this.Divine diviner =
//                let valueType' = valueType.Divine diviner :?> Type
//                diviner.ValueWithName (valueName, valueType')

//    type DivinablePropertyGet (this' : IDivinable option, declaringType : IDivinable, propertyName : string) =
//        override this.ToString () =
//            sprintf "PropertyGet (%A, %A, %A)" this' declaringType propertyName

//        interface IDivinable with
//            member this.Divine diviner =
//                let this'' =
//                    match this' with
//                    | Some t -> t.Divine diviner
//                    | None -> null
//                let declaringType' = declaringType.Divine diviner :?> Type
//                diviner.PropertyGet (this'', declaringType', propertyName)

//    type DivinableCall (this' : IDivinable option, declaringType : IDivinable, methodName : string, typeArguments : IDivinable list, arguments : IDivinable list) =
//        override this.ToString () =
//            sprintf "Call (%A, %A, %A, %A)" this' declaringType methodName arguments

//        interface IDivinable with
//            member this.Divine diviner =
//                let this'' =
//                    match this' with
//                    | Some t -> t.Divine diviner
//                    | None -> null
//                let declaringType' = declaringType.Divine diviner :?> Type
//                let typeArguments' = typeArguments |> List.map (fun t -> t.Divine diviner :?> Type)
//                let arguments' = arguments |> List.map (fun a -> a.Divine diviner)
//                diviner.Call (this'', declaringType', methodName, typeArguments', arguments')

//    type DivinableCoerce (toCoerce : IDivinable, coercionType : IDivinable) =
//        override this.ToString () =
//            sprintf "Coerce (%A, %A)" toCoerce coercionType

//        interface IDivinable with
//            member this.Divine diviner =
//                let toCoerce' = toCoerce.Divine diviner
//                let coercionType' = coercionType.Divine diviner :?> Type
//                diviner.Coerce (toCoerce', coercionType')

//    type DivinableNewUnionCase(declaringType : IDivinable, unionCaseName : string, unionCases : IDivinable list) =
//        override this.ToString () =
//            sprintf "NewUnionCase (%A, %A, %A)" declaringType unionCaseName unionCases

//        interface IDivinable with
//            member this.Divine diviner =
//                let declaringType' = declaringType.Divine diviner :?> Type
//                let unionCases' = unionCases |> List.map (fun u -> u.Divine diviner)
//                diviner.NewUnionCase (declaringType', unionCaseName, unionCases')

//    type DivinableLambda (parameterName : string, parameterType : IDivinable, body : IDivinable) =
//        override this.ToString () =
//            sprintf "Lambda (%A, %A, %A)" parameterName parameterType body

//        interface IDivinable with
//            member this.Divine diviner =
//                let parameterType' = parameterType.Divine diviner :?> Type
//                let var = Var (parameterName, parameterType', false)
//                let f =
//                    fun argument ->
//                        body.Divine diviner
//                box f

//    type Divinable () =
//        static member Type (name : string) =
//            DivinableType name :> IDivinable

//        static member AssemblyType (assemblyType : Type) =
//            Divinable.Type assemblyType.AssemblyQualifiedName

//        static member Value (value : obj, valueType : IDivinable) =
//            DivinableValue (value, valueType) :> IDivinable

//        static member ValueWithName (name : string, valueType : IDivinable) =
//            DivinableValueWithName (name, valueType) :> IDivinable

//        static member PropertyGet (this : IDivinable option, declaringType : IDivinable, propertyName : string) =
//            DivinablePropertyGet (this, declaringType, propertyName) :> IDivinable

//        static member Call (this : IDivinable option, declaringType : IDivinable, methodName : string, typeArguments : IDivinable list, arguments : IDivinable list) =
//            DivinableCall (this, declaringType, methodName, typeArguments, arguments) :> IDivinable

//        static member Coerce (toCoerce : IDivinable, coercionType : IDivinable) =
//            DivinableCoerce (toCoerce, coercionType) :> IDivinable

//        static member NewUnionCase (declaringType : IDivinable, unionCaseName : string, unionCases : IDivinable list) =
//            DivinableNewUnionCase (declaringType, unionCaseName, unionCases) :> IDivinable

//    type Divinable<'T> (raw : Divinable) =
//        class
//        member this.Raw : Divinable = raw
//        end

//    type NewDivineBuilder () =
//        member this.Return (value : 'T) : IDivinable = 
//            printfn "Wrapping a raw value into an Divinable"
//            Divinable.Value (value, (Divinable.Type typeof<'T>.AssemblyQualifiedName))

//        member this.Quote (expr : Expr<IDivinable>) =
//            expr

//        member this.Run (expr : Expr<IDivinable>) =
//            let returnMethodInfo = typeof<NewDivineBuilder>.GetMethod "Return"

//            let rec transformExpr recognizedVars toTransform =
//                match toTransform with
//                | Patterns.Call (Some _, returnMethodInfo, [innerExpr]) ->
//                    transformExpr recognizedVars innerExpr

//                | Patterns.Call (this', methodInfo, argumentExprs) ->
//                    let this'' = match this' with
//                                 | Some t ->
//                                     Some (transformExpr recognizedVars t)
//                                 | None -> None
//                    let declaringType = Divinable.AssemblyType methodInfo.DeclaringType
//                    let methodName = methodInfo.Name
//                    let typeArguments = methodInfo.GetGenericArguments () |> Seq.map (fun t -> Divinable.AssemblyType t) |> Seq.toList
//                    let arguments = argumentExprs |> List.map (transformExpr recognizedVars)
//                    Divinable.Call (this'', declaringType, methodName, typeArguments, arguments)

//                | Patterns.Value (value, valueType) ->
//                    Divinable.Value (value, Divinable.AssemblyType valueType)

//                | Patterns.Coerce (toCoerce, coercionType) ->
//                    Divinable.Coerce (transformExpr recognizedVars toCoerce, Divinable.AssemblyType coercionType)

//                | Patterns.PropertyGet (this', propertyInfo, indexerArgumentExprs) ->
//                    let this'' = match this' with
//                                 | Some t ->
//                                     Some (transformExpr recognizedVars t)
//                                 | None -> None
//                    let declaringType = Divinable.AssemblyType propertyInfo.DeclaringType
//                    let propertyName = propertyInfo.Name
//                    Divinable.PropertyGet (this'', declaringType, propertyName)

//                | Patterns.NewUnionCase (unionCaseInfo, unionCases) ->
//                    let declaringType = Divinable.AssemblyType unionCaseInfo.DeclaringType
//                    let unionCaseName = unionCaseInfo.Name
//                    let unionCases' = unionCases |> List.map (transformExpr recognizedVars)
//                    Divinable.NewUnionCase (declaringType, unionCaseName, unionCases')

//                //| Patterns.Lambda (parameterVar, bodyExpr) ->
//                //    let parameterName = parameterVar.Name
//                //    let parameterType = Divinable.AssemblyType parameterVar.Type
//                //    let body = transformExpr recognizedVars bodyExpr
//                //    Divinable.Lambda (parameterName, parameterType, body)

//                | Patterns.Let (letVar, letExpr, inExpr) ->
//                    let recognizedVars' = recognizedVars |> Map.add letVar.Name (transformExpr recognizedVars letExpr)
//                    transformExpr recognizedVars' inExpr

//                | Patterns.Var var ->
//                    match recognizedVars.TryFind var.Name with
//                    | Some divinable -> divinable
//                    | None -> Divinable.Value (null, Divinable.AssemblyType typeof<obj>)
//                    //| None -> raise (Exception (sprintf "Unrecognized var: %A" var))

//                | unrecognizedExpr ->
//                    raise (Exception (sprintf "Unrecognized expression: %A" unrecognizedExpr))

//            transformExpr Map.empty expr

//    let newDivine = new NewDivineBuilder ()