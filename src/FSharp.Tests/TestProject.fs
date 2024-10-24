namespace FSharp.Tests

open Fixie

type TestProject () =
  interface ITestProject with
    member this.Configure(configuration: TestConfiguration, environment: TestEnvironment) =
      configuration.Conventions.Add<TestModuleDiscovery, ParallelExecution>()