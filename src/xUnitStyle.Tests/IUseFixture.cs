namespace xUnitStyle.Tests
{
    public interface IUseFixture<T> where T : new()
    {
        void SetFixture(T data);
    }
}