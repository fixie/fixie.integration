namespace CustomConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Shouldly;

    class SkipMethodTests : IDisposable
    {
        readonly Calculator calculator;

        public SkipMethodTests()
        {
            calculator = new Calculator();
            Log.WhereAmI();
        }

        public void ShouldAdd()
        {
            Log.WhereAmI();
            calculator.Add(2, 3).ShouldBe(5);
        }

        [Skip]
        public void ShouldBeSkipped()
        {
            throw new Exception(nameof(ShouldBeSkipped) + " was invoked explicitly.");
        }

        public void ShouldSubtract()
        {
            Log.WhereAmI();
            calculator.Subtract(5, 3).ShouldBe(2);
        }

        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}