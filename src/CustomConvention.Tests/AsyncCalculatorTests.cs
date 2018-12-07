namespace CustomConvention.Tests
{
    using System;
    using System.Threading.Tasks;
    using Fixie.Integration;
    using Shouldly;

    public class AsyncCalculatorTests : IDisposable
    {
        Calculator calculator;
        string test;

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
            test = nameof(ShouldAdd);
            calculator.Add(2, 3).ShouldBe(5);
        }

        public async Task ShouldSubtract()
        {
            await Awaited();
            Log.WhereAmI();
            test = nameof(ShouldSubtract);
            calculator.Subtract(5, 3).ShouldBe(2);
        }

        public async Task TearDown()
        {
            await Awaited();
            Log.WhereAmI();
        }

        public void Dispose()
        {
            Log.WhereAmI();
        }

        static Task Awaited() => Task.FromResult(0);
    }
}