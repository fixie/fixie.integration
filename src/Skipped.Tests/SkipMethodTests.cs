namespace Skipped.Tests
{
    using System;
    using System.Text;
    using Fixie.Integration;
    using Shouldly;

    public class SkipMethodTests : IDisposable
    {
        readonly Calculator calculator;
        readonly StringBuilder log;

        public SkipMethodTests()
        {
            calculator = new Calculator();
            log = new StringBuilder();
            log.WhereAmI();
        }

        public void ShouldAdd()
        {
            log.WhereAmI();
            calculator.Add(2, 3).ShouldBe(5);
        }

        [Skip]
        public void ShouldBeSkipped()
        {
            throw new Exception(nameof(ShouldBeSkipped) + " was invoked explicitly.");
        }

        public void ShouldSubtract()
        {
            log.WhereAmI();
            calculator.Subtract(5, 3).ShouldBe(2);
        }

        public void Dispose()
        {
            log.WhereAmI();
            log.ShouldHaveLines(
                ".ctor",
                "ShouldAdd",
                "ShouldSubtract",
                "Dispose");
        }
    }
}