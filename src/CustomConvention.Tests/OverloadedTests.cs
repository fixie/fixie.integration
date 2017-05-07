namespace CustomConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Should;

    class OverloadedTests : IDisposable
    {
        readonly Calculator calculator;

        public OverloadedTests()
        {
            calculator = new Calculator();
            Log.WhereAmI();
        }

        public void ShouldAdd()
        {
            Log.WhereAmI();
            calculator.Add(2, 3).ShouldEqual(5);
        }
        
        public void ShouldSubtract()
        {
            Log.WhereAmI();
            calculator.Subtract(2, 3).ShouldEqual(-1);
        }

        [Input(2, 3, 5)]
        [Input(3, 5, 8)]
        public void ShouldAdd(int a, int b, int expectedSum)
        {
            Log.WhereAmI(new object[] { a, b, expectedSum });
            calculator.Add(a, b).ShouldEqual(expectedSum);
        }
        
        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}
