namespace Fixie.Integration
{
    public interface IDatabase
    {
        string Query();
    }

    public class RealDatabase : IDatabase
    {
        public string Query() => nameof(RealDatabase);
    }
}