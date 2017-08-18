namespace x86.Tests
{
    using System;
    using Shouldly;

    class x86Tests
    {
        public void ShouldRunTestsInAssembliesTargetingx86()
        {
            (IntPtr.Size * 8).ShouldBe(32);
        }
    }
}
