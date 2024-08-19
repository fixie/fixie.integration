using System.Reflection;
using Fixie;
using Fixie.Integration;

namespace CustomConvention.Tests;

public class TestProject : ITestProject
{
    static readonly string[] LifecycleMethods = {"SetUp", "TearDown"};

    public void Configure(TestConfiguration configuration, TestEnvironment environment)
    {
        var discovery = new CustomDiscovery(environment);
        var execution = new CustomExecution();

        configuration.Conventions.Add(discovery, execution);

        configuration.Reports.Add(new XUnitV2XmlReport(environment));
    }

    class CustomDiscovery : IDiscovery
    {
        readonly bool shouldRunAll;
        readonly IReadOnlyList<string> desiredCategories;

        public CustomDiscovery(TestEnvironment environment)
        {
            desiredCategories = environment.CustomArguments;
            shouldRunAll = !desiredCategories.Any();
        }

        public IEnumerable<Type> TestClasses(IEnumerable<Type> concreteClasses)
            => concreteClasses.Where(x => x.Name.EndsWith("Tests"));

        public IEnumerable<MethodInfo> TestMethods(IEnumerable<MethodInfo> publicMethods)
            => publicMethods
                .Where(x => !LifecycleMethods.Contains(x.Name))
                .Where(x => shouldRunAll || MethodHasAnyDesiredCategory(x, desiredCategories));

        static bool MethodHasAnyDesiredCategory(MethodInfo method, IReadOnlyList<string> desiredCategories)
            => Categories(method).Any(testCategory => desiredCategories.Contains(testCategory.Name));

        static CategoryAttribute[] Categories(MethodInfo method)
            => method.GetCustomAttributes<CategoryAttribute>(true).ToArray();
    }

    class CustomExecution : IExecution
    {
        public async Task Run(TestSuite testSuite)
        {
            var executeExplicitSkip = SingleTestWasRequested(testSuite);

            foreach (var testClass in testSuite.TestClasses)
            {
                foreach (var test in testClass.Tests)
                {
                    if (test.Method.Has<SkipAttribute>() && !executeExplicitSkip)
                        continue;

                    using var ioc = InitContainerForIntegrationTests();

                    var instance = ioc.Construct(testClass.Type);

                    await TryLifecycleMethod(testClass, instance, "SetUp");

                    if (test.HasParameters)
                    {
                        foreach (var parameters in UsingInputAttributes(test))
                            await test.Run(instance, parameters);
                    }
                    else
                    {
                        await test.Run(instance);
                    }

                    await TryLifecycleMethod(testClass, instance, "TearDown");
                }
            }
        }

        static bool SingleTestWasRequested(TestSuite testSuite)
        {
            var testClasses = testSuite.TestClasses;

            return testClasses.Count == 1 &&
                   testClasses.Single().Tests.Count == 1;
        }

        static async Task TryLifecycleMethod(TestClass testClass, object instance, string name)
        {
            var method = testClass.Type.GetMethod(name);

            if (method != null)
                await method.Call(instance);
        }

        static IEnumerable<object[]> UsingInputAttributes(Test test)
            => test.GetAll<InputAttribute>().Select(input => input.Parameters);

        static IoCContainer InitContainerForIntegrationTests()
        {
            var container = new IoCContainer();
            container.Add(typeof(IDatabase), typeof(RealDatabase));
            container.Add(typeof(IThirdPartyService), typeof(FakeThirdPartyService));
            return container;
        }
    }
}

class FakeThirdPartyService : IThirdPartyService
{
    public string Invoke() => nameof(FakeThirdPartyService);
}