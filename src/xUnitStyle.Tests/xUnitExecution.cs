namespace xUnitStyle.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Fixie;
    using Fixie.Integration;

    public class xUnitExecution : IExecution
    {
        public async Task Run(TestSuite testSuite)
        {
            foreach (var testClass in testSuite.TestClasses)
            {
                var fixtures = PrepareFixtureData(testClass.Type);

                foreach (var test in testClass.Tests)
                {
                    var instance = testClass.Construct();

                    foreach (var injectionMethod in fixtures.Keys)
                        injectionMethod.Invoke(instance, new[] {fixtures[injectionMethod]});

                    await test.Run(instance);

                    await instance.DisposeWhenApplicable();
                }

                foreach (var fixtureInstance in fixtures.Values)
                    await fixtureInstance.DisposeWhenApplicable();
            }
        }

        static Dictionary<MethodInfo, object> PrepareFixtureData(Type testClass)
        {
            var fixtures = new Dictionary<MethodInfo, object>();

            foreach (var @interface in FixtureInterfaces(testClass))
            {
                var fixtureDataType = @interface.GetGenericArguments()[0];

                var fixtureInstance = Activator.CreateInstance(fixtureDataType);

                var method = @interface.GetMethod("SetFixture", new[] {fixtureDataType});
                fixtures[method] = fixtureInstance;
            }

            return fixtures;
        }

        static IEnumerable<Type> FixtureInterfaces(Type testClass)
        {
            return testClass.GetInterfaces()
                .Where(@interface => @interface.IsGenericType &&
                                     @interface.GetGenericTypeDefinition() == typeof(IUseFixture<>));
        }
    }
}