using Fixie.Integration;

namespace CustomConvention.Tests;

class CalculatorTests
{
    Calculator? calculator;

    public CalculatorTests()
    {
    }

    public void SetUp()
    {
        calculator = new Calculator();
    }

    [CategoryA]
    public void ShouldAdd()
    {
        calculator.ShouldNotBeNull();
        calculator.Add(2, 3).ShouldBe(5);
    }

    [CategoryB]
    public void ShouldSubtract()
    {
        calculator.ShouldNotBeNull();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    [Input(2, 3, 5)]
    [Input(3, 5, 8)]
    [CategoryA]
    public void ShouldAdd(int a, int b, int expectedSum)
    {
        calculator.ShouldNotBeNull();
        calculator.Add(a, b).ShouldBe(expectedSum);
    }

    [Input(1, 2, typeof(int))]
    [Input(2L, 4L, typeof(long))]
    public void ShouldInferGenericTypes<T>(T a, T b, Type expectedT)
    {
        typeof(T).ShouldBe(expectedT, $"Expected T to resolve to type {expectedT}, but it resolved to type {typeof(T)} instead.");
    }

    public void ShouldFail()
    {
        throw new Exception("This test is written to fail, to demonstrate failure reporting.");
    }

    [Skip]
    public void ShouldBeSkipped()
    {
        throw new Exception(nameof(ShouldBeSkipped) + " was invoked explicitly.");
    }

    public void TearDown()
    {
    }
}