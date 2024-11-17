using Fixie.Integration;

namespace CustomConvention.Tests;

class IoCTests
{
    readonly IDatabase database;
    readonly IThirdPartyService service;

    public IoCTests(IDatabase database, IThirdPartyService service)
    {
        this.database = database;
        this.service = service;
    }

    public void ShouldReceiveRealDatabase()
    {
        database.Query().ShouldBe("RealDatabase");
    }

    public void ShouldReceiveFakeThirdPartyService()
    {
        service.Invoke().ShouldBe("FakeThirdPartyService");
    }
}