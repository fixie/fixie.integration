namespace DefaultConvention.Tests
{
    using Newtonsoft.Json;
    using Shouldly;

    class DependenciesTests
    {
        public void CanLoadExpectedVersionOfDependentAssembly()
        {
            //For .NET Core test assemblies, Fixie brings in a transitive
            //dependency for Newtonsoft.Json.

            //Assert that the version in effect at runtime is the one
            //explicitly referenced by the Fixie.Integration project in
            //this solution.

            typeof(JsonConvert).Assembly.GetName().Version.ToString()
                .ShouldBe("11.0.0.0");

            var sample = JsonConvert.DeserializeObject<Sample>("{Property:\"Value\"}");
            sample.Property.ShouldBe("Value");
        }

        class Sample
        {
            public string Property { get; set; }
        }
    }
}