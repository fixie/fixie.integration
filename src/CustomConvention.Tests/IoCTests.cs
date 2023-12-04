namespace CustomConvention.Tests;

using Fixie.Integration;
using Shouldly;

class IoCTests
{
    readonly IDatabase database;
    readonly IThirdPartyService service;

    public IoCTests(IDatabase database, IThirdPartyService service)
    {
        this.database = database;
        this.service = service;
        Log.WhereAmI();
    }

    public void ShouldReceiveRealDatabase()
    {
        Log.WhereAmI();
        database.Query().ShouldBe("RealDatabase");
    }

    public void ShouldReceiveFakeThirdPartyService()
    {
        Log.WhereAmI();
        service.Invoke().ShouldBe("FakeThirdPartyService");
    }
}