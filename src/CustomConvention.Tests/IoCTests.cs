namespace CustomConvention.Tests
{
    using System;
    using System.Text;
    using Fixie.Integration;
    using Shouldly;

    public class IoCTests : IDisposable
    {
        readonly IDatabase database;
        readonly IThirdPartyService service;
        readonly StringBuilder log;
        string test;

        public IoCTests(IDatabase database, IThirdPartyService service)
        {
            this.database = database;
            this.service = service;
            log = new StringBuilder();
            log.WhereAmI();
        }

        public void ShouldReceiveRealDatabase()
        {
            log.WhereAmI();
            test = nameof(ShouldReceiveRealDatabase);
            database.Query().ShouldBe("RealDatabase");
        }

        public void ShouldReceiveFakeThirdPartyService()
        {
            log.WhereAmI();
            test = nameof(ShouldReceiveFakeThirdPartyService);
            service.Invoke().ShouldBe("FakeThirdPartyService");
        }

        public void Dispose()
        {
            log.WhereAmI();
            log.ShouldHaveLines(
                ".ctor",
                test,
                "Dispose");
        }
    }
}