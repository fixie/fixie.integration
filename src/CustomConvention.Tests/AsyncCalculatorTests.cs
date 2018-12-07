namespace CustomConvention.Tests
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
        string test;

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
            test = nameof(ShouldAdd);
            calculator.Add(2, 3).ShouldBe(5);
        }

        public async Task ShouldSubtract()
        {
            await Awaited();
            log.WhereAmI();
            test = nameof(ShouldSubtract);
            calculator.Subtract(5, 3).ShouldBe(2);
        }

        public async Task TearDown()
        {
            await Awaited();
            log.WhereAmI();
        }

        public void Dispose()
        {
            log.WhereAmI();
            log.ShouldHaveLines(
                ".ctor",
                "SetUp",
                test,
                "TearDown",
                "Dispose");
        }

        static Task Awaited() => Task.FromResult(0);
    }
}