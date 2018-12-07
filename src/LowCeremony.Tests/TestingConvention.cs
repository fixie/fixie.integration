namespace LowCeremony.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        static readonly string[] LifecycleMethods = { "SetUp", "TearDown" };

        public TestingConvention()
        {
            Methods
                .Where(x => !LifecycleMethods.Contains(x.Name))
                .OrderBy(x => x.Name, StringComparer.Ordinal);
        }

        public void Execute(TestClass testClass)
        {
            void Execute(object instance, string method)
            {
                var query = testClass.Type
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                    .Where(x => x.Name == method);

                foreach (var q in query)
                    q.Execute(instance);
            }

            testClass.RunCases(@case =>
            {
                var instance = testClass.Construct();

                Execute(instance, "SetUp");
                @case.Execute(instance);
                Execute(instance, "TearDown");

                instance.Dispose();
            });
        }
    }
}