namespace Divination.Test

open System
open NUnit.Framework
open FsUnit
open Divination
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.Evaluator
open System.Reflection
open System.Collections.Generic

//type DerpClass () =
//    static member x = 1
//    static member y = 2

//module DerpModule =
//    let x = 3
//    let y = 4

//    let added z =
//        x + y + z

//    let useList ls =
//        ls |> Seq.map (fun l -> "Hello " + l)

[<TestFixture>]
module DivinableBuilderTest =
    let diviner = Diviner ()
    let divineContext = { DivineContext.Variables = Map.empty }
    let divine = Divinable.divine diviner divineContext

    [<Test>]
    let ``DivinableBuilder does something`` () =
        let divinable =
            divinable {
                let hello = "Hello"
                let world = "World"
                return hello + " " + world
            }
        let divined = divine divinable
        divined.Value |> should equal "Hello World"

    [<Test>]
    let ``DivinableBuilder allows combining Divinables`` () =
        let hello =
            divinable {
                return "Hello"
            }
        let divinable =
            divinable {
                let! h = hello
                return h + " World"
            }
        let divined = divine divinable
        divined.Value |> should equal "Hello World"
        
//    [<Test>]
//    let ``Playground`` () =
//        let divinable : IDivinable<int> = Divinable.value 5
//        let divineHellos : IDivinable<string> =
//            divine {
//                let people = ["Alice"; "Bob"]
//                let hellos = people |> Seq.map (fun l -> "Hello " + l)
//                let! (divinedValue : int) = divinable
//                return hellos |> String.concat "\n"
//            }
//        let divined = divineHellos.Divine (Diviner ())
//        divined.Value |> should equal "Hello Alice\nHello Bob"

    //[<Test>]
    //let ``Playground`` () =
    //    let diviner = Divination.NewDivinable.DefaultDiviner ()
    //    let derpType = Divination.NewDivinable.Divinable.Type typeof<DerpClass>.AssemblyQualifiedName

    //    printfn "%A" (derpType.Divine diviner)

    //    let derpX = Divination.NewDivinable.Divinable.PropertyGet (None, derpType, "x")
    //    let derpY = Divination.NewDivinable.Divinable.PropertyGet (None, derpType, "y")

    //    printfn "x: %A" (derpX.Divine diviner)
    //    printfn "y: %A" (derpY.Divine diviner)

    //    let intType = Divination.NewDivinable.Divinable.Type typeof<int>.AssemblyQualifiedName

    //    let typeFromOperator operatorExpression =
    //        match operatorExpression with
    //        | FSharp.Quotations.Patterns.Lambda (_, FSharp.Quotations.Patterns.Lambda (_, FSharp.Quotations.Patterns.Call(_, methodInfo, _))) ->
    //            methodInfo.DeclaringType
    //        | _ -> null

    //    let operatorsType = Divination.NewDivinable.Divinable.Type (typeFromOperator <@ (+) @>).AssemblyQualifiedName
    //    let add = Divination.NewDivinable.Divinable.Call (None, operatorsType, "op_Addition", [intType; intType; intType], [derpX; derpY])
    //    printfn "x + y: %A" (add.Divine diviner)

    //    let derpAdded = newDivine {
    //        let total = DerpModule.added 5
    //        return total
    //    }
    //    printfn "derpAdded: %A" derpAdded
    //    printfn "added: %A" (derpAdded.Divine diviner)

    //    let derpUseList = newDivine {
    //        let hellos = DerpModule.useList ["Alice"; "Bob"]
    //        return String.concat "\n" hellos
    //    }
    //    derpUseList.Divine diviner |> should equal "Hello Alice\nHello Bob"

    //[<Test>]
    //let ``Divinable stuff`` () =
    //    //let divined1 = Divinable.FSharpExpr <@ 5 @>
    //    //let value1 = divined1 |> eval
    //    //value1 |> should equal 5

    //    let divined2 = divine { return 5 }
    //    let value2 = divined2 |> eval
    //    value2 |> should equal 5
    //    match divined2 with
    //    | Divinable.FSharpExpr e ->
    //        e |> should equal <@ 5 @>

    //    let divineWut = divine { return 5 + 2 }
    //    let valueWut = divineWut |> eval
    //    valueWut |> should equal 3
    //    match divineWut with
    //    | Divinable.FSharpExpr e ->
    //        e |> should equal <@ 5 - 2 @>

        //let divined3 =
        //    divine {
        //        let r = Random ()
        //        return r.Next ()
        //    }
        //match divined3 with
        //| Divinable.FSharpExpr e ->
        //    e |> should equal <@ (Random ()).Next () @>

    //[<Test>]
    //let ``Divinable stuff 2`` () =
    //    let divineFunc x y =
    //        divine {
    //            let! xVal = x
    //            let! yVal = y
    //            return xVal + yVal
    //        }

    //    let divined =
    //        divine {
    //            return! divineFunc (Literal 1) (Literal 2)
    //        }
    //    let value = match divined with | Literal a -> a
    //    value |> should equal 3