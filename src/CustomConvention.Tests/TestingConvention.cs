namespace CustomConvention.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Fixie;
    using Fixie.Integration;

    class TestingConvention : Discovery, Execution
    {
        static readonly string[] LifecycleMethods = { "SetUp", "TearDown" };
        readonly bool shouldRunAll;
        readonly string[] desiredCategories;

        public TestingConvention(string[] customArguments)
        {
            desiredCategories = customArguments;
            shouldRunAll = !desiredCategories.Any();
        }

        public IEnumerable<Type> TestClasses(IEnumerable<Type> concreteClasses)
            => concreteClasses;

        public IEnumerable<MethodInfo> TestMethods(IEnumerable<MethodInfo> publicMethods)
            => publicMethods
                .Where(x => !LifecycleMethods.Contains(x.Name))
                .Where(x => shouldRunAll || MethodHasAnyDesiredCategory(x, desiredCategories))
                .Shuffle();

        public async Task RunAsync(TestAssembly testAssembly)
        {
            var executeExplicitSkip = SingleTestWasRequested(testAssembly);

            foreach (var testClass in testAssembly.TestClasses)
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
                            await test.RunAsync(instance, parameters);
                    }
                    else
                    {
                        await test.RunAsync(instance);
                    }

                    await TryLifecycleMethod(testClass, instance, "TearDown");
                }
            }
        }

        static bool SingleTestWasRequested(TestAssembly testAssembly)
        {
            var testClasses = testAssembly.TestClasses;
            
            return testClasses.Count == 1 &&
                   testClasses.Single().Tests.Count == 1;
        }

        static async Task TryLifecycleMethod(TestClass testClass, object instance, string name)
        {
            var method = testClass.Type.GetMethod(name);
            
            if (method != null)
                await method.CallAsync(instance);
        }

        static IEnumerable<object[]> UsingInputAttributes(Test test)
            => test.GetAll<InputAttribute>().Select(input => input.Parameters);

        static bool MethodHasAnyDesiredCategory(MethodInfo method, string[] desiredCategories)
            => Categories(method).Any(testCategory => desiredCategories.Contains(testCategory.Name));

        static CategoryAttribute[] Categories(MethodInfo method)
            => method.GetCustomAttributes<CategoryAttribute>(true).ToArray();

        static IoCContainer InitContainerForIntegrationTests()
        {
            var container = new IoCContainer();
            container.Add(typeof(IDatabase), typeof(RealDatabase));
            container.Add(typeof(IThirdPartyService), typeof(FakeThirdPartyService));
            return container;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class InputAttribute : Attribute
    {
        public InputAttribute(params object[] parameters)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    class SkipAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    abstract class CategoryAttribute : Attribute
    {
        public string Name => GetType().Name.Replace("Attribute", "");
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    class CategoryAAttribute : CategoryAttribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    class CategoryBAttribute : CategoryAttribute { }

    class FakeThirdPartyService : IThirdPartyService
    {
        public string Invoke() => nameof(FakeThirdPartyService);
    }
}