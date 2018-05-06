namespace Skipped.Tests
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SkipAttribute : Attribute { }
}