namespace Nested.Tests
{
    using System;
    using System.Text;
    using Fixie.Integration;
    using Shouldly;

    public class CalculatorTests
    {
        class AddingTests : IDisposable
        {
            readonly Calculator calculator;
            readonly StringBuilder log;

            public AddingTests()
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

            public void Dispose()
            {
                log.WhereAmI();

                log.ShouldHaveLines(
                    ".ctor",
                    "ShouldAdd",
                    "Dispose");
            }
        }

        class SubtractingTests : IDisposable
        {
            readonly Calculator calculator;
            readonly StringBuilder log;

            public SubtractingTests()
            {
                calculator = new Calculator();
                log = new StringBuilder();
                log.WhereAmI();
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
                    "ShouldSubtract",
                    "Dispose");
            }
        }
    }
}