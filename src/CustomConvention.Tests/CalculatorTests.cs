namespace CustomConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Shouldly;

    public class CalculatorTests : IDisposable
    {
        readonly Calculator calculator;

        public CalculatorTests()
        {
            calculator = new Calculator();
            Log.WhereAmI();
        }

        public void SetUp()
        {
            Log.WhereAmI();
        }

        public void ShouldAdd()
        {
            Log.WhereAmI();
            calculator.Add(2, 3).ShouldBe(5);
        }

        public void ShouldSubtract()
        {
            Log.WhereAmI();
            calculator.Subtract(5, 3).ShouldBe(2);
        }

        public void TearDown()
        {
            Log.WhereAmI();
        }

        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}