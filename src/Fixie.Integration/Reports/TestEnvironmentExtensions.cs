namespace Fixie.Integration.Reports;

public static class TestEnvironmentExtensions
{
    public static void ConfigureReports(this TestConfiguration configuration, TestEnvironment environment)
    {
        if (environment.IsContinuousIntegration())
        {
            configuration.Reports.Add(new XUnitV2XmlReport(environment));
            configuration.Reports.Add(new JsonReport(environment));
            configuration.Reports.Add(new GitHubReport(environment));
        }
    }
}