namespace FSharp.Tests

open Fixie
open Fixie.Integration.Reports

type TestProject () =
  interface ITestProject with
    member this.Configure(configuration: TestConfiguration, environment: TestEnvironment) =
      configuration.Conventions.Add<TestModuleDiscovery, ParallelExecution>()
      configuration.ConfigureReports(environment);
