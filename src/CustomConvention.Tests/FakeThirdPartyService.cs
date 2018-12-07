namespace CustomConvention.Tests
{
    using Fixie.Integration;

    class FakeThirdPartyService : IThirdPartyService
    {
        public string Invoke() => nameof(FakeThirdPartyService);
    }
}