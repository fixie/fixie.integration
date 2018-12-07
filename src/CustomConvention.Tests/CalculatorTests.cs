namespace CustomConvention.Tests
{
    using System;
    using System.Text;
    using Fixie.Integration;
    using Shouldly;

    public class CalculatorTests : IDisposable
    {
        readonly Calculator calculator;
        readonly StringBuilder log;
        string test;

        public CalculatorTests()
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
            test = nameof(ShouldAdd);
            calculator.Add(2, 3).ShouldBe(5);
        }

        public void ShouldSubtract()
        {
            log.WhereAmI();
            test = nameof(ShouldSubtract);
            calculator.Subtract(5, 3).ShouldBe(2);
        }

        public void TearDown()
        {
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
    }
}