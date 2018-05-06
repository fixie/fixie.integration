namespace Categories.Tests
{
    using Fixie.Integration;
    using Shouldly;

    public class CalculatorTests
    {
        readonly Calculator calculator;

        public CalculatorTests()
        {
            calculator = new Calculator();
        }

        [CategoryA]
        public void ShouldAdd()
        {
            calculator.Add(2, 3).ShouldBe(5);
        }

        [CategoryB]
        public void ShouldSubtract()
        {
            calculator.Add(2, 3).ShouldBe(5);
        }
    }
}