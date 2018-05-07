namespace CustomConvention.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    class TestingConvention : Discovery, Execution
    {
        public TestingConvention()
        {
            Methods
                .OrderBy(x => x.Name, StringComparer.Ordinal);

            Parameters
                .Add<InputAttributeParameterSource>();
        }

        public void Execute(TestClass testClass)
        {
            var methodWasExplicitlyRequested = testClass.TargetMethod != null;

            var skipClass = testClass.Type.Has<SkipAttribute>() && !methodWasExplicitlyRequested;

            var instance = skipClass ? null : testClass.Construct();

            testClass.RunCases(@case =>
            {
                var skipMethod = @case.Method.Has<SkipAttribute>() && !methodWasExplicitlyRequested;

                if (skipClass)
                    @case.Skip("Whole class skipped");
                else if (!skipMethod)
                    @case.Execute(instance);
            });

            instance.Dispose();
        }
    }

    class InputAttributeParameterSource : ParameterSource
    {
        public IEnumerable<object[]> GetParameters(MethodInfo method)
            => method.GetCustomAttributes<InputAttribute>(true).Select(input => input.Parameters);
    }
}