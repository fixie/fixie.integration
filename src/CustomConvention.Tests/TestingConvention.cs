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
        readonly bool shouldRunAll;
        readonly string[] desiredCategories;

        public TestingConvention(string[] customArguments)
        {
            desiredCategories = customArguments;
            shouldRunAll = !desiredCategories.Any();
        }

        public IEnumerable<MethodInfo> TestMethods(IEnumerable<MethodInfo> publicMethods)
            => publicMethods
                .Where(x => !LifecycleMethods.Contains(x.Name))
                .Where(x => shouldRunAll || MethodHasAnyDesiredCategory(x, desiredCategories))
                .Shuffle();

        public void Execute(TestClass testClass)
        {
            var methodWasExplicitlyRequested = testClass.TargetMethod != null;

            foreach (var test in testClass.Tests)
            {
                var instance = testClass.Type.IsStatic() ? null : ioc.Construct(testClass.Type);

                if (methodWasExplicitlyRequested || !test.Method.Has<SkipAttribute>())
                {
                    testClass.Type.GetMethod("SetUp")?.Execute(instance);
                    test.RunCases(UsingInputAttributes, instance);
                    testClass.Type.GetMethod("TearDown")?.Execute(instance);
                }

                instance.Dispose();
            }
        }

        static IEnumerable<object[]> UsingInputAttributes(MethodInfo method)
            => method.GetCustomAttributes<InputAttribute>(true).Select(input => input.Parameters);

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