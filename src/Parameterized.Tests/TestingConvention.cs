namespace Parameterized.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;

    public class TestingConvention : Discovery, Execution
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
            var instance = testClass.Construct();

            testClass.RunCases(@case => @case.Execute(instance));

            instance.Dispose();
        }

        class InputAttributeParameterSource : ParameterSource
        {
            public IEnumerable<object[]> GetParameters(MethodInfo method)
            {
                return method.GetCustomAttributes<InputAttribute>(true).Select(input => input.Parameters);
            }
        }
    }
}