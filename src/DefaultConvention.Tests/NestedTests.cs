namespace DefaultConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Shouldly;

    public class NestedTests
    {
        class AddingTests : IDisposable
        {
            readonly Calculator calculator;

            public AddingTests()
            {
                calculator = new Calculator();
                Log.WhereAmI();
            }

            public void ShouldAdd()
            {
                Log.WhereAmI();
                calculator.Add(2, 3).ShouldBe(5);
            }

            public void Dispose()
            {
                Log.WhereAmI();
            }
        }

        class SubtractingTests : IDisposable
        {
            readonly Calculator calculator;

            public SubtractingTests()
            {
                calculator = new Calculator();
                Log.WhereAmI();
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
}