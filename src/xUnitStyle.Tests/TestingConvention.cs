namespace xUnitStyle.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;
    
    public class TestingConvention : Discovery, Execution
    {
        public IEnumerable<Type> TestClasses(IEnumerable<Type> concreteClasses)
            => concreteClasses
                .Where(HasAnyFactMethods);

        public IEnumerable<MethodInfo> TestMethods(IEnumerable<MethodInfo> publicMethods)
            => publicMethods
                .Where(x => x.Has<FactAttribute>())
                .Shuffle();

        public void Execute(TestClass testClass)
        {
            var fixtures = PrepareFixtureData(testClass.Type);

            foreach (var test in testClass.Tests)
            {
                var instance = testClass.Construct();

                foreach (var injectionMethod in fixtures.Keys)
                    injectionMethod.Invoke(instance, new[] { fixtures[injectionMethod] });

                test.Run(instance);

                instance.Dispose();
            };

            foreach (var fixtureInstance in fixtures.Values)
                fixtureInstance.Dispose();
        }

        static Dictionary<MethodInfo, object> PrepareFixtureData(Type testClass)
        {
            var fixtures = new Dictionary<MethodInfo, object>();

            foreach (var @interface in FixtureInterfaces(testClass))
            {
                var fixtureDataType = @interface.GetGenericArguments()[0];

                var fixtureInstance = Activator.CreateInstance(fixtureDataType);

                var method = @interface.GetMethod("SetFixture", new[] { fixtureDataType });
                fixtures[method] = fixtureInstance;
            }

            return fixtures;
        }

        bool HasAnyFactMethods(Type type)
        {
            const BindingFlags publicMethods =
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            return type.GetMethods(publicMethods).Any(x => x.Has<FactAttribute>());
        }

        static IEnumerable<Type> FixtureInterfaces(Type testClass)
        {
            return testClass.GetInterfaces()
                            .Where(@interface => @interface.IsGenericType &&
                                                 @interface.GetGenericTypeDefinition() == typeof(IUseFixture<>));
        }
    }
}