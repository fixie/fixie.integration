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

        public TestingConvention()
        {
            Methods
                .Where(x => !LifecycleMethods.Contains(x.Name))
                .OrderBy(x => x.Name, StringComparer.Ordinal);

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
    }

    class InputAttributeParameterSource : ParameterSource
    {
        public IEnumerable<object[]> GetParameters(MethodInfo method)
            => method.GetCustomAttributes<InputAttribute>(true).Select(input => input.Parameters);
    }
}