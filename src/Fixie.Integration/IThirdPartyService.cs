namespace Fixie.Integration
{
    public interface IThirdPartyService
    {
        string Invoke();
    }

    public class RealThirdPartyService : IThirdPartyService
    {
        public string Invoke() => nameof(RealThirdPartyService);
    }
}