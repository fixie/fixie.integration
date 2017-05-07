namespace CustomConvention.Tests
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    class SkipAttribute : Attribute
    {
        public string Reason { get; set; }
    }
}