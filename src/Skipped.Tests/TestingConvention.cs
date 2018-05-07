namespace Skipped.Tests
{
    using System;
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        public TestingConvention()
        {
            Methods
                .OrderBy(x => x.Name, StringComparer.Ordinal);
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
}