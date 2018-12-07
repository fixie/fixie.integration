namespace CustomConvention.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    class TestingConvention : Discovery, Execution
    {
        static readonly string[] LifecycleMethods = { "SetUp", "TearDown" };

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

            var skipClass = testClass.Type.Has<SkipAttribute>() && !methodWasExplicitlyRequested;

            testClass.RunCases(@case =>
            {
                var instance = skipClass ? null : testClass.Construct();

                var skipMethod = @case.Method.Has<SkipAttribute>() && !methodWasExplicitlyRequested;

                if (skipClass)
                {
                    @case.Skip("Whole class skipped");
                }
                else if (!skipMethod)
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