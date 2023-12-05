namespace xUnitStyle.Tests;

public interface IUseFixture<T> where T : class, new()
{
    void SetFixture(T data);
}