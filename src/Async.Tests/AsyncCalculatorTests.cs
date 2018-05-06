namespace Async.Tests
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Fixie.Integration;
    using Shouldly;

    public class AsyncCalculatorTests : IDisposable
    {
        Calculator calculator;
        readonly StringBuilder log;

        public AsyncCalculatorTests()
        {
            log = new StringBuilder();
            log.WhereAmI();
        }

        public async Task SetUp()
        {
            await Awaited();
            log.WhereAmI();
            calculator = new Calculator();
        }

        public async Task ShouldAdd()
        {
            await Awaited();
            log.WhereAmI();
            calculator.Add(2, 3).ShouldBe(5);
        }

        static Task Awaited() => Task.FromResult(0);

        public void Dispose()
        {
            log.WhereAmI();
            log.ShouldHaveLines(
                ".ctor",
                "SetUp",
                "ShouldAdd",
                "Dispose");
        }
    }
}