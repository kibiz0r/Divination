namespace Divination

open System
open System.Reflection
open System.Collections.Generic
    
module MethodInfoExtensions =
    type MethodInfo with
        member this.InvokeGeneric<'T> (this' : obj, arguments : obj []) =
            let rec resolve (a : Type) (f : Type) (acc : Dictionary<_,_>) =
                if f.IsGenericParameter then
                    if not (acc.ContainsKey (f)) then acc.Add (f, a)
                else 
                    Array.zip (a.GetGenericArguments ()) (f.GetGenericArguments ())
                    |> Array.iter (fun (act, form) -> resolve act form acc)

            let invokeMethod (m : MethodInfo) args =
                let m = if m.ContainsGenericParameters then
                            let typeMap = new Dictionary<_,_> ()
                            let parameters = m.GetParameters ()
                            Array.zip args parameters
                                |> Array.iter (fun (a, f) -> 
                                    resolve (a.GetType ()) f.ParameterType typeMap
                                )
                            let returnType = m.ReturnType
                            if returnType.IsGenericParameter && not <| typeMap.ContainsKey returnType then
                                typeMap.Add (returnType, typeof<'T>)
                            let genericArguments = m.GetGenericArguments ()
                            let actuals =
                                genericArguments
                                |> Array.map (fun formal -> typeMap.[formal])
                            m.MakeGenericMethod (actuals)
                        else 
                            m
                m.Invoke(this', args)
            invokeMethod this arguments
