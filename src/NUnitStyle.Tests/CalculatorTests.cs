using Fixie.Integration;

namespace NUnitStyle.Tests;

[TestFixture]
public class CalculatorTests
{
    Calculator? calculator;

    public CalculatorTests()
    {
    }

    [TestFixtureSetUp]
    public void TestFixtureSetUp()
    {
        calculator = new Calculator();
    }

    [SetUp]
    public void SetUp()
    {
    }

    [Test]
    public void ShouldAdd()
    {
        calculator.ShouldNotBeNull();
        calculator.Add(2, 3).ShouldBe(5);
    }

    [Test]
    public void ShouldSubtract()
    {
        calculator.ShouldNotBeNull();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    [Test]
    public void ShouldDivide()
    {
        calculator.ShouldNotBeNull();
        calculator.Divide(6, 3).ShouldBe(2);
    }

    [Test]
    [ExpectedException(typeof(DivideByZeroException))]
    public void ShouldThrowWhenDividingByZero()
    {
        calculator.ShouldNotBeNull();
        calculator.Divide(1, 0);
    }

    [TearDown]
    public void TearDown()
    {
    }

    [TestFixtureTearDown]
    public void TestFixtureTearDown()
    {
    }
}