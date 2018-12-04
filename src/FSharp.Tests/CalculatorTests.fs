module FSharp.Tests.CalculatorTests

open Fixie.Integration
open Shouldly

let ``Should Add``() =
    let calculator = new Calculator()
    calculator.Add(2, 3).ShouldBe(5)

let ``Should Subtract``() =
    let calculator = new Calculator()
    calculator.Subtract(5, 3).ShouldBe(2)