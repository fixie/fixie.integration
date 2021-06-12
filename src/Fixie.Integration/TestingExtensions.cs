using System;

namespace Fixie.Integration
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class TestingExtensions
    {
        public static bool Has<TAttribute>(this MemberInfo member) where TAttribute : Attribute
        {
            return member.GetCustomAttributes<TAttribute>(true).Any();
        }

        public static async Task DisposeWhenApplicableAsync(this object instance)
        {
            if (instance is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();

            if (instance is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
