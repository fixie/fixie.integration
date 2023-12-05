namespace NUnitStyle.Tests;

[AttributeUsage(AttributeTargets.Class)]
public class TestFixtureAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class TestFixtureSetUpAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SetUpAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class TearDownAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class TestFixtureTearDownAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ExpectedExceptionAttribute(Type exceptionType) : Attribute
{
    public Type ExpectedException { get; set; } = exceptionType;

    public string? ExpectedMessage { get; set; }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseSourceAttribute : Attribute
{
    public TestCaseSourceAttribute(string sourceName, Type sourceType)
    {
        SourceName = sourceName;
        SourceType = sourceType;
    }

    public TestCaseSourceAttribute(string sourceName)
    {
        SourceName = sourceName;
        SourceType = null;
    }

    public Type? SourceType { get; set; }
    public string SourceName { get; private set; }
}