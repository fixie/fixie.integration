namespace CustomConvention.Tests
{
    using System;
    using Fixie.Integration;
    using Shouldly;

    class IoCTests : IDisposable
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

        public void Dispose()
        {
            Log.WhereAmI();
        }
    }
}