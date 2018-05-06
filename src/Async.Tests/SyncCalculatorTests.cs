namespace Async.Tests
{
    using System;
    using System.Text;
    using Fixie.Integration;
    using Shouldly;

    public class SyncCalculatorTests : IDisposable
    {
        readonly Calculator calculator;
        readonly StringBuilder log;

        public SyncCalculatorTests()
        {
            calculator = new Calculator();
            log = new StringBuilder();
            log.WhereAmI();
        }

        public void SetUp()
        {
            log.WhereAmI();
        }

        public void ShouldAdd()
        {
            log.WhereAmI();
            calculator.Add(2, 3).ShouldBe(5);
        }

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