namespace CustomConvention.Tests
{
    using Fixie.Integration;

    public class FakeDatabase : IDatabase
    {
        public string Query() => nameof(FakeDatabase);
    }
}