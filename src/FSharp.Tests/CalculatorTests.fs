module FSharp.Tests.CalculatorTests

open Fixie.Integration
open Fixie.Assertions

let ``Should Add``() =
    let calculator = new Calculator()
    calculator.Add(2, 3).ShouldBe(5, (*In F#, we have to explicitly specify the expression parameter*)"calculator.Add(2, 3)")

let ``Should Subtract``() =
    let calculator = new Calculator()
    calculator.Subtract(5, 3).ShouldBe(2, (*In F#, we have to explicitly specify the expression parameter*)"calculator.Subtract(5, 3)")