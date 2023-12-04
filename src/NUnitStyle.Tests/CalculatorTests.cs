using System;
using Fixie.Integration;
using Shouldly;

namespace NUnitStyle.Tests;

[TestFixture]
public class CalculatorTests
{
    Calculator calculator;

    public CalculatorTests()
    {
        Log.WhereAmI();
    }

    [TestFixtureSetUp]
    public void TestFixtureSetUp()
    {
        Log.WhereAmI();
        calculator = new Calculator();
    }

    [SetUp]
    public void SetUp()
    {
        Log.WhereAmI();
    }

    [Test]
    public void ShouldAdd()
    {
        Log.WhereAmI();
        calculator.Add(2, 3).ShouldBe(5);
    }

    [Test]
    public void ShouldSubtract()
    {
        Log.WhereAmI();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    [Test]
    public void ShouldDivide()
    {
        Log.WhereAmI();
        calculator.Divide(6, 3).ShouldBe(2);
    }

    [Test]
    [ExpectedException(typeof(DivideByZeroException))]
    public void ShouldThrowWhenDividingByZero()
    {
        Log.WhereAmI();
        calculator.Divide(1, 0);
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