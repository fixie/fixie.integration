using System;

namespace Fixie.Integration;

using System.Threading.Tasks;

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