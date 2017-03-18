namespace Divination

open System
open System.Reflection
open FSharp.Reflection
open Divination

type FSharpDiviner<'Identifier> () =
    inherit Diviner<'Identifier> ()

    static let mutable current : IFSharpDiviner<'Identifier> option = None
    static member Current
        with get () =
            match current with
            | Some c -> c
            | None ->
                let c = FSharpDiviner<'Identifier> () :> IFSharpDiviner<'Identifier>
                current <- Some c
                c
        and set (value) =
            current <- Some value

    interface IFSharpDiviner<'Identifier>

    override this.NewContext (scope) =
        DivinationContext<'Identifier> () :> _

    override this.ResolveContext (context) =
        context.Resolve this

    //interface IDiviner<IDivinationContext<'Identifier>, IIdentificationScope<'Identifier>, 'Identifier> with
    //    member this.Divine<'T> (scope, divinable) =
    //        this.Divine<'T> (scope, divinable)

    override this.Identifier (scope, identifier) =
        raise (NotImplementedException "FSharpDiviner does not handle Identifiers")

    override this.Call (scope, this', methodInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.Identify (scope, t)
            | None -> null
        let arguments' = List.map (fun argument -> this.Identify (scope, argument)) arguments |> List.toArray
        methodInfo.Invoke (this'', arguments')

    default this.NewObject (scope, constructorInfo, arguments) =
        let arguments' = List.map (fun argument -> this.Identify (scope, argument)) arguments |> List.toArray
        constructorInfo.Invoke (arguments')

    default this.PropertyGet (scope, this', propertyInfo, arguments) =
        let this'' =
            match this' with
            | Some t -> this.Identify (scope, t)
            | None -> null
        let arguments' = List.map (fun argument -> this.Identify (scope, argument)) arguments |> List.toArray
        propertyInfo.GetValue (this'', arguments')

    default this.Value (scope, value, type') =
        Convert.ChangeType (value, type')

    default this.Coerce (scope, argument, type') =
        Convert.ChangeType (this.Identify (scope, argument), type')

    default this.NewUnionCase (scope, unionCaseInfo, arguments) =
        let arguments' = List.map (fun a -> this.Identify (scope, a)) arguments |> List.toArray
        FSharpValue.MakeUnion (unionCaseInfo, arguments')

    default this.Var (scope, name) =
        invalidOp (sprintf "Unbound Var: %s" name)

    default this.Let (scope, var, argument, body) =
        this.Identify (IdentificationScope.add var argument scope, body)

type FSharpDiviner = FSharpDiviner<obj>