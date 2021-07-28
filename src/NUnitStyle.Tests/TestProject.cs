namespace NUnitStyle.Tests
{
    using Fixie;

    public class TestProject : ITestProject
    {
        public void Configure(TestConfiguration configuration, TestEnvironment environment)
        {
            configuration.Conventions.Add<NUnitDiscovery, NUnitExecution>();
        }
    }
}
