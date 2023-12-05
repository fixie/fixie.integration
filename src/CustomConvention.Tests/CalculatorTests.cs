using Fixie.Integration;
using Shouldly;

namespace CustomConvention.Tests;

class CalculatorTests
{
    Calculator? calculator;

    public CalculatorTests()
    {
        Log.WhereAmI();
    }

    public void SetUp()
    {
        Log.WhereAmI();
        calculator = new Calculator();
    }

    [CategoryA]
    public void ShouldAdd()
    {
        Log.WhereAmI();
        calculator.ShouldNotBeNull();
        calculator.Add(2, 3).ShouldBe(5);
    }

    [CategoryB]
    public void ShouldSubtract()
    {
        Log.WhereAmI();
        calculator.ShouldNotBeNull();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    [Input(2, 3, 5)]
    [Input(3, 5, 8)]
    [CategoryA]
    public void ShouldAdd(int a, int b, int expectedSum)
    {
        Log.WhereAmI(new object[] { a, b, expectedSum });
        calculator.ShouldNotBeNull();
        calculator.Add(a, b).ShouldBe(expectedSum);
    }

    [Input(1, 2, typeof(int))]
    [Input(2L, 4L, typeof(long))]
    public void ShouldInferGenericTypes<T>(T a, T b, Type expectedT)
    {
        Log.WhereAmI<T>(new object?[] { a, b, expectedT });
        typeof(T).ShouldBe(expectedT, $"Expected T to resolve to type {expectedT}, but it resolved to type {typeof(T)} instead.");
    }

    public void ShouldFail()
    {
        Log.WhereAmI();
        throw new Exception("This test is written to fail, to demonstrate failure reporting.");
    }

    [Skip]
    public void ShouldBeSkipped()
    {
        Log.WhereAmI();
        throw new Exception(nameof(ShouldBeSkipped) + " was invoked explicitly.");
    }

    public void TearDown()
    {
        Log.WhereAmI();
    }
}