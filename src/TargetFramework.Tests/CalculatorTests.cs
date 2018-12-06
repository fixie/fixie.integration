namespace TargetFramework.Tests
{
    using Fixie.Integration;
    using Shouldly;

    class CalculatorTests
    {
        public void ShouldAdd()
        {
            var calculator = new Calculator();
            calculator.Add(2, 3).ShouldBe(5);
        }

        public void ShouldSubtract()
        {
            var calculator = new Calculator();
            calculator.Subtract(5, 3).ShouldBe(2);
        }
    }
}
