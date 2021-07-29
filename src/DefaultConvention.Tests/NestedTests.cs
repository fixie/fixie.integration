namespace DefaultConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Shouldly;

    public class NestedTests
    {
        class AddingTests
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
        }

        class SubtractingTests
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
        }
    }
}