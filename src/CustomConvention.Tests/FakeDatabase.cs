namespace CustomConvention.Tests
{
    using Fixie.Integration;

    class FakeDatabase : IDatabase
    {
        public string Query() => nameof(FakeDatabase);
    }
}