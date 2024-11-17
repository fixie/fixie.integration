using Fixie.Integration;

namespace xUnitStyle.Tests;

public class CalculatorTests : IUseFixture<FixtureData>, IUseFixture<DisposableFixtureData>, IDisposable
{
    readonly Calculator calculator;

    bool executedAddTest = false;
    bool executedSubtractTest = false;

    FixtureData? fixtureData;
    DisposableFixtureData? disposableFixtureData;

    public CalculatorTests()
    {
        calculator = new Calculator();
    }

    public void SetFixture(FixtureData data)
    {
        fixtureData = data;
        data.Instance.ShouldBe(1);
    }

    public void SetFixture(DisposableFixtureData data)
    {
        disposableFixtureData = data;
        data.Instance.ShouldBe(1);
    }

    [Fact]
    public void ShouldAdd()
    {
        executedAddTest = true;
        calculator.Add(2, 3).ShouldBe(5);
    }

    [Fact]
    public void ShouldSubtract()
    {
        executedSubtractTest = true;
        calculator.Subtract(5, 3).ShouldBe(2);
    }

    public void Dispose()
    {
        (executedAddTest && executedSubtractTest).ShouldBe(false);
        (executedAddTest || executedSubtractTest).ShouldBe(true);
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