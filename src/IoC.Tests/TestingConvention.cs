namespace IoC.Tests
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
            using (var container = InitContainerForIntegrationTests())
            {
                var instance = container.Get(testClass.Type);

                testClass.RunCases(@case => @case.Execute(instance));
            }
        }

        static IoCContainer InitContainerForIntegrationTests()
        {
            var container = new IoCContainer();
            container.Add(typeof(IDatabase), typeof(RealDatabase));
            container.Add(typeof(IThirdPartyService), typeof(FakeThirdPartyService));
            return container;
        }
    }
}