using Fixie.Integration;

namespace DefaultConvention.Tests;

abstract class BaseTests
{
    public void BaseTest() => Log.WhereAmI();
}

class FirstChildTests : BaseTests
{
    public void ChildTest() => Log.WhereAmI();
}

class SecondChildTests : BaseTests
{
    public void ChildTest() => Log.WhereAmI();
}