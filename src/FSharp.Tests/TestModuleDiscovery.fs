namespace FSharp.Tests

open System
open System.Collections.Generic
open System.Reflection
open Fixie

type TestModuleDiscovery () =
  interface IDiscovery with
    member this.TestClasses(concreteClasses: IEnumerable<Type>) =
      Seq.filter (fun x -> x.Name.EndsWith("Tests")) concreteClasses;

    member this.TestMethods(publicMethods: IEnumerable<MethodInfo>) =
      Seq.filter (fun x -> x.IsStatic) publicMethods