using Fixie.Integration;

namespace CustomConvention.Tests;

class AsyncCalculatorTests
{
    Calculator? calculator;

    public AsyncCalculatorTests()
    {
    }

    public async Task SetUp()
    {
        await Awaited();
        calculator = new Calculator();
    }

    public async Task ShouldAdd()
    {
        await Awaited();
        calculator.ShouldNotBeNull();
        calculator.Add(2, 3).ShouldBe(5);
    }

    public async Task ShouldSubtract()
    {
        await Awaited();
        calculator.ShouldNotBeNull();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    public async Task TearDown()
    {
        await Awaited();
    }

    static Task Awaited() => Task.FromResult(0);
}