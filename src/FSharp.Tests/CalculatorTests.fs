module FSharp.Tests.CalculatorTests

open Fixie.Integration
open Shouldly

let ShouldAdd() =
    let calculator = new Calculator()
    calculator.Add(2, 3).ShouldBe(5)

let ShouldSubtract() =
    let calculator = new Calculator()
    calculator.Subtract(5, 3).ShouldBe(2)