using Fixie.Integration;

namespace CustomConvention.Tests;

class AsyncCalculatorTests
{
    Calculator? calculator;

    public AsyncCalculatorTests()
    {
        Log.WhereAmI();
    }

    public async Task SetUp()
    {
        await Awaited();
        Log.WhereAmI();
        calculator = new Calculator();
    }

    public async Task ShouldAdd()
    {
        await Awaited();
        Log.WhereAmI();
        calculator.ShouldNotBeNull();
        calculator.Add(2, 3).ShouldBe(5);
    }

    public async Task ShouldSubtract()
    {
        await Awaited();
        Log.WhereAmI();
        calculator.ShouldNotBeNull();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    public async Task TearDown()
    {
        await Awaited();
        Log.WhereAmI();
    }

    static Task Awaited() => Task.FromResult(0);
}