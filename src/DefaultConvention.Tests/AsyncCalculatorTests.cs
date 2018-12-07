namespace DefaultConvention.Tests
{
    using System;
    using System.Threading.Tasks;
    using Fixie.Integration;
    using Shouldly;

    class AsyncCalculatorTests : IDisposable
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

        public void Dispose()
        {
            Log.WhereAmI();
        }

        static Task Awaited() => Task.FromResult(0);
    }
}