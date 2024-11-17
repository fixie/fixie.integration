using Fixie;
using Fixie.Integration.Reports;

namespace DefaultConvention.Tests;

public class TestProject : ITestProject
{
    public void Configure(TestConfiguration configuration, TestEnvironment environment) =>
        configuration.ConfigureReports(environment);
}