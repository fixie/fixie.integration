namespace Async.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        public TestingConvention()
        {
            Methods
                .Where(x => x.Name != "SetUp")
                .OrderBy(x => x.Name, StringComparer.Ordinal);
        }

        public void Execute(TestClass testClass)
        {
            testClass.RunCases(@case =>
            {
                var instance = testClass.Construct();

                SetUp(instance);

                @case.Execute(instance);

                instance.Dispose();
            });
        }

        static void SetUp(object instance)
        {
            var query = instance.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(x => x.Name == "SetUp");

            foreach (var q in query)
                q.Execute(instance);
        }
    }
}