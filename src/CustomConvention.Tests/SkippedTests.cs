namespace CustomConvention.Tests
{
    using System;
    using Fixie.Integration;

    class SkippedTests : IDisposable
    {
        public SkippedTests()
        {
            Log.WhereAmI();
        }

        [Skip(Reason="Skipped with reason.")]
        public void SkippedWithReason()
        {
            Log.WhereAmI();
            throw new Exception();
        }

        [Skip]
        public void SkippedWithoutReason()
        {
            Log.WhereAmI();
            throw new Exception();
        }

        [Explicit]
        public void OnlyRunsWhenExplicitlyInvoked()
        {
            Log.WhereAmI();
        }

        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}