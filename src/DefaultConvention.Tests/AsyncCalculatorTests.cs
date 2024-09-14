using Fixie.Integration;

namespace DefaultConvention.Tests;

class AsyncCalculatorTests
{
    readonly Calculator calculator;

    public AsyncCalculatorTests()
    {
        calculator = new Calculator();
        Log.WhereAmI();
    }

    public async Task ShouldAdd()
    {
        await Awaited();
        Log.WhereAmI();
        calculator.Add(2, 3).ShouldBe(5);
    }

    public async Task ShouldSubtract()
    {
        await Awaited();
        Log.WhereAmI();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    static Task Awaited() => Task.FromResult(0);
}