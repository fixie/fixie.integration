namespace DefaultConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Should;

    class CalculatorTests : IDisposable
    {
        public CalculatorTests()
        {
            Log.WhereAmI();
        }

        public void ShouldAdd()
        {
            Log.WhereAmI();
            var calculator = new Calculator();
            calculator.Add(2, 3).ShouldEqual(5);
        }

        public void ShouldSubtract()
        {
            Log.WhereAmI();
            var calculator = new Calculator();
            calculator.Subtract(5, 3).ShouldEqual(2);
        }

        public void ShouldFail()
        {
            Log.WhereAmI();
            throw new Exception("This test is written to fail, to demonstrate failure reporting.");
        }

        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}
