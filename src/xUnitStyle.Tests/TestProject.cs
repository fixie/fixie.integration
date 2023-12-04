using Fixie;

namespace xUnitStyle.Tests;

public class TestProject : ITestProject
{
    public void Configure(TestConfiguration configuration, TestEnvironment environment)
    {
        configuration.Conventions.Add<xUnitDiscovery, xUnitExecution>();
    }
}