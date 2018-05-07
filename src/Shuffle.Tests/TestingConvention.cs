namespace Shuffle.Tests
{
    using System;
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        const int Seed = 8675309;

        public TestingConvention()
        {
            Methods
                .Shuffle(new Random(Seed));
        }

        public void Execute(TestClass testClass)
        {
            var instance = testClass.Construct();

            testClass.RunCases(@case => @case.Execute(instance));

            instance.Dispose();
        }
    }
}