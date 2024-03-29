﻿using System.Reflection;
using Fixie;

namespace NUnitStyle.Tests;

public class NUnitDiscovery : IDiscovery
{
    public IEnumerable<Type> TestClasses(IEnumerable<Type> concreteClasses)
        => concreteClasses.Where(x => ReflectionExtensions.Has<TestFixtureAttribute>(x));

    public IEnumerable<MethodInfo> TestMethods(IEnumerable<MethodInfo> publicMethods)
        => publicMethods
            .Where(x => x.Has<TestAttribute>())
            .OrderBy(x => x.Name, StringComparer.Ordinal);
}