namespace Skipped.Tests
{
    using System;

    [Skip]
    public class SkipClassTests
    {
        public void FirstSkip()
            => throw new Exception(nameof(FirstSkip) + " was invoked explicitly.");

        public void SecondSkip()
            => throw new Exception(nameof(SecondSkip) + " was invoked explicitly.");
    }
}