namespace LowCeremony.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        static readonly string[] LifecycleMethods = { "FixtureSetUp", "FixtureTearDown", "SetUp", "TearDown" };

        public TestingConvention()
        {
            Methods
                .Where(x => !LifecycleMethods.Contains(x.Name))
                .OrderBy(x => x.Name, StringComparer.Ordinal);
        }

        public void Execute(TestClass testClass)
        {
            var instance = testClass.Construct();

            void Execute(string method)
            {
                var query = testClass.Type
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                    .Where(x => x.Name == method);

                foreach (var q in query)
                    q.Execute(instance);
            }

            Execute("FixtureSetUp");
            testClass.RunCases(@case =>
            {
                Execute("SetUp");
                @case.Execute(instance);
                Execute("TearDown");
            });
            Execute("FixtureTearDown");

            instance.Dispose();
        }
    }
}