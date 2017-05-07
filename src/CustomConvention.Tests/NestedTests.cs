namespace CustomConvention.Tests
{
    using Fixie.Integration;

    class NestedTests
    {
        class InnerTests
        {
            public void InnerTest() => Log.WhereAmI();

            class InnerInnerTests
            {
                public void InnerInnerTest() => Log.WhereAmI();
            }
        }
    }
}