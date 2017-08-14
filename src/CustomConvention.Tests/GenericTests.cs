namespace CustomConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Shouldly;

    class GenericTests : IDisposable
    {
        public GenericTests()
        {
            Log.WhereAmI();
        }

        [Input(1, 2, typeof(int))]
        [Input(2L, 4L, typeof(long))]
        public void ShouldInferGenericTypes<T>(T a, T b, Type expectedT)
        {
            Log.WhereAmI<T>(new object[] { a, b, expectedT });
            typeof(T).ShouldBe(expectedT, $"Expected T to resolve to type {expectedT}, but it resolved to type {typeof(T)} instead.");
        }

        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}