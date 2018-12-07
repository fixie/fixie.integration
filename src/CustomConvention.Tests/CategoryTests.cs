namespace CustomConvention.Tests
{
    using Fixie.Integration;
    using Shouldly;

    class CategoryTests
    {
        [CategoryA]
        public void ShouldAdd()
        {
            var calculator = new Calculator();
            calculator.Add(2, 3).ShouldBe(5);
        }

        [CategoryB]
        public void ShouldSubtract()
        {
            var calculator = new Calculator();
            calculator.Add(2, 3).ShouldBe(5);
        }
    }
}