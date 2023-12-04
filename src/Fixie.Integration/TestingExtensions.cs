using System;
using System.Threading.Tasks;

namespace Fixie.Integration;

public static class TestingExtensions
{
    public static async Task DisposeWhenApplicable(this object instance)
    {
        if (instance is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();

        if (instance is IDisposable disposable)
            disposable.Dispose();
    }
}