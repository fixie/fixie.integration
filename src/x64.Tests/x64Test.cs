namespace x64.Tests
{
    using System;
    using Shouldly;

    class x64Tests
    {
        public void ShouldRunTestsInAssembliesTargetingx64()
        {
            (IntPtr.Size * 8).ShouldBe(64);
        }
    }
}
