namespace CustomConvention.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;
    using Fixie.Integration;

    class TestingConvention : Discovery, Execution, IDisposable
    {
        static readonly string[] LifecycleMethods = { "SetUp", "TearDown" };
        readonly IoCContainer ioc = InitContainerForIntegrationTests();

        public TestingConvention(string[] include)
        {
            var desiredCategories = include;
            var shouldRunAll = !desiredCategories.Any();

            Methods
                .Where(x => !LifecycleMethods.Contains(x.Name))
                .Where(x => shouldRunAll || MethodHasAnyDesiredCategory(x, desiredCategories))
                .Shuffle();

            Parameters
                .Add<InputAttributeParameterSource>();
        }

        public void Execute(TestClass testClass)
        {
            var methodWasExplicitlyRequested = testClass.TargetMethod != null;

            testClass.RunCases(@case =>
            {
                var instance = testClass.Type.IsStatic() ? null : ioc.Construct(testClass.Type);

                if (methodWasExplicitlyRequested || !@case.Method.Has<SkipAttribute>())
                {
                    testClass.Type.GetMethod("SetUp")?.Execute(instance);
                    @case.Execute(instance);
                    testClass.Type.GetMethod("TearDown")?.Execute(instance);
                }

                instance.Dispose();
            });
        }

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

        public void Dispose()
        {
            ioc.Dispose();
        }
    }

    class InputAttributeParameterSource : ParameterSource
    {
        public IEnumerable<object[]> GetParameters(MethodInfo method)
            => method.GetCustomAttributes<InputAttribute>(true).Select(input => input.Parameters);
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
}