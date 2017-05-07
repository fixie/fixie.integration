namespace CustomConvention.Tests
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    class ExplicitAttribute : Attribute { }
}