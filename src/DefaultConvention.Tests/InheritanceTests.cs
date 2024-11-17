namespace DefaultConvention.Tests;

abstract class BaseTests
{
    public void BaseTest() { }
}

class FirstChildTests : BaseTests
{
    public void ChildTest() { }
}

class SecondChildTests : BaseTests
{
    public void ChildTest() { }
}