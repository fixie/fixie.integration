namespace CustomConvention.Tests
{
    using System;
    using Fixie.Integration;

    class SkipMethodTests : IDisposable
    {
        public SkipMethodTests()
        {
            Log.WhereAmI();
        }

        [Skip]
        public void ShouldBeSkipped()
        {
            throw new Exception(nameof(ShouldBeSkipped) + " was invoked explicitly.");
        }

        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}