namespace CustomConvention.Tests
{
    using System;
    using System.Text;
    using Fixie.Integration;
    using Shouldly;

    public class SkipMethodTests : IDisposable
    {
        readonly Calculator calculator;
        readonly StringBuilder log;
        string test;

        public SkipMethodTests()
        {
            calculator = new Calculator();
            log = new StringBuilder();
            log.WhereAmI();
        }

        public void ShouldAdd()
        {
            log.WhereAmI();
            test = nameof(ShouldAdd);
            calculator.Add(2, 3).ShouldBe(5);
        }

        [Skip]
        public void ShouldBeSkipped()
        {
            test = nameof(ShouldBeSkipped);
            throw new Exception(test + " was invoked explicitly.");
        }

        public void ShouldSubtract()
        {
            log.WhereAmI();
            test = nameof(ShouldSubtract);
            calculator.Subtract(5, 3).ShouldBe(2);
        }

        public void Dispose()
        {
            log.WhereAmI();

            if (test == null)
                log.ShouldHaveLines(".ctor", "Dispose");
            else
                log.ShouldHaveLines(".ctor", test, "Dispose");
        }
    }
}