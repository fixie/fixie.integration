using Fixie.Integration;
using Shouldly;

namespace xUnitStyle.Tests;

public class CalculatorTests : IUseFixture<FixtureData>, IUseFixture<DisposableFixtureData>, IDisposable
{
    readonly Calculator calculator;

    bool executedAddTest = false;
    bool executedSubtractTest = false;

    FixtureData fixtureData;
    DisposableFixtureData disposableFixtureData;

    public CalculatorTests()
    {
        calculator = new Calculator();
        Log.WhereAmI();
    }

    public void SetFixture(FixtureData data)
    {
        Log.WhereAmI();
        fixtureData = data;
        Log.WriteLine("   FixtureData " + fixtureData.Instance);
        data.Instance.ShouldBe(1);
    }

    public void SetFixture(DisposableFixtureData data)
    {
        Log.WhereAmI();
        disposableFixtureData = data;
        Log.WriteLine("   DisposableFixtureData " + disposableFixtureData.Instance);
        data.Instance.ShouldBe(1);
    }

    [Fact]
    public void ShouldAdd()
    {
        executedAddTest = true;
        Log.WhereAmI();
        calculator.Add(2, 3).ShouldBe(5);
    }

    [Fact]
    public void ShouldSubtract()
    {
        executedSubtractTest = true;
        Log.WhereAmI();
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    public void Dispose()
    {
        Log.WhereAmI();
        (executedAddTest && executedSubtractTest).ShouldBeFalse();
        (executedAddTest || executedSubtractTest).ShouldBeTrue();
    }
}

public class DisposableFixtureData : IDisposable
{
    static int InstanceCounter;
    public static int DisposalCount { get; private set; }

    public int Instance { get; }

    public DisposableFixtureData()
    {
        Instance = ++InstanceCounter;
    }

    public void Dispose()
    {
        DisposalCount++;
    }
}

public class FixtureData
{
    static int InstanceCounter;

    public int Instance { get; }

    public FixtureData()
    {
        Instance = ++InstanceCounter;
    }
}