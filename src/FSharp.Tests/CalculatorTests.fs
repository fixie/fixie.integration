namespace FSharp.Tests

open Fixie.Integration
open Shouldly

type CalculatorTests () =

    member this.ShouldAdd() =
        let calculator = new Calculator()
        calculator.Add(2, 3).ShouldBe(5)

    member this.ShouldSubtract() =
        let calculator = new Calculator()
        calculator.Subtract(5, 3).ShouldBe(2)