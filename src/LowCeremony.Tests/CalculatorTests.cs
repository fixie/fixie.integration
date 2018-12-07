namespace LowCeremony.Tests
{
    using System;
    using System.Text;
    using Fixie.Integration;
    using Shouldly;

    public class CalculatorTests : IDisposable
    {
        readonly Calculator calculator;
        readonly StringBuilder log;
        string operation;

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
            operation = "Add";
            calculator.Add(2, 3).ShouldBe(5);
        }

        public void ShouldSubtract()
        {
            log.WhereAmI();
            operation = "Subtract";
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
                $"Should{operation}",
                "TearDown",
                "Dispose");
        }
    }
}