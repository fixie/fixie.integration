using System.Collections.Generic;
using Fixie.Integration;
using Shouldly;

namespace NUnitStyle.Tests;

[TestFixture]
public class TestCaseSourceAttributeCalculatorTests
{
    readonly Calculator calculator;

    public TestCaseSourceAttributeCalculatorTests()
    {
        calculator = new Calculator();
        Log.WhereAmI();
    }

    [TestFixtureSetUp]
    public void TestFixtureSetUp()
    {
        Log.WhereAmI();
    }

    [SetUp]
    public void SetUp()
    {
        Log.WhereAmI();
    }

    public static IEnumerable<object[]> FieldSource = new List<object[]>
    {
        new object[] { "Internal Field", 1, 2, 3 },
        new object[] { "Internal Field", 2, 3, 5 }
    };

    public static IEnumerable<object[]> MethodSource()
    {
        return new List<object[]>
        {
            new object[] { "Internal Method", 3, 4, 7 },
            new object[] { "Internal Method", 4, 5, 9 }
        };
    }

    public static IEnumerable<object[]> PropertySource
    {
        get
        {
            return new List<object[]>
            {
                new object[] { "Internal Property", 5, 6, 11 },
                new object[] { "Internal Property", 6, 7, 13 }
            };
        }
    }

    [Test]
    [TestCaseSource("FieldSource", typeof(ExternalSourceOfTestCaseData))]
    [TestCaseSource("MethodSource", typeof(ExternalSourceOfTestCaseData))]
    [TestCaseSource("PropertySource", typeof(ExternalSourceOfTestCaseData))]
    [TestCaseSource("FieldSource")]
    [TestCaseSource("MethodSource")]
    [TestCaseSource("PropertySource")]
    public void ShouldAddFromFieldSource(string source, int a, int b, int expectedSum)
    {
        Log.WriteLine($"{source}: ShouldAdd({a}, {b}, {expectedSum})");
        calculator.Add(a, b).ShouldBe(expectedSum);
    }

    [TearDown]
    public void TearDown()
    {
        Log.WhereAmI();
    }

    [TestFixtureTearDown]
    public void TestFixtureTearDown()
    {
        Log.WhereAmI();
    }
}

public class ExternalSourceOfTestCaseData
{
    public static IEnumerable<object[]> FieldSource = new List<object[]>
    {
        new object[] { "External Field", 10, 20, 30 },
        new object[] { "External Field", 20, 30, 50 }
    };

    public static IEnumerable<object[]> MethodSource() => new List<object[]>
    {
        new object[] { "External Method", 30, 40, 70 },
        new object[] { "External Method", 40, 50, 90 }
    };

    public static IEnumerable<object[]> PropertySource => new List<object[]>
    {
        new object[] { "External Property", 50, 60, 110 },
        new object[] { "External Property", 60, 70, 130 }
    };
}