using Fixie.Integration;

namespace DefaultConvention.Tests;

public class NestedTests
{
    class AddingTests
    {
        readonly Calculator calculator;

        public AddingTests()
        {
            calculator = new Calculator();
        }

        public void ShouldAdd()
        {
            calculator.Add(2, 3).ShouldBe(5);
        }
    }

    class SubtractingTests
    {
        readonly Calculator calculator;

        public SubtractingTests()
        {
            calculator = new Calculator();
        }

        public void ShouldSubtract()
        {
            calculator.Subtract(5, 3).ShouldBe(2);
        }
    }
}