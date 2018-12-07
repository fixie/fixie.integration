namespace CustomConvention.Tests
{
    using Fixie.Integration;

    public class FakeThirdPartyService : IThirdPartyService
    {
        public string Invoke() => nameof(FakeThirdPartyService);
    }
}