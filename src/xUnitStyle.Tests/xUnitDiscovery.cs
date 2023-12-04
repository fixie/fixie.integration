namespace xUnitStyle.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fixie;

public class xUnitDiscovery : IDiscovery
{
    public IEnumerable<Type> TestClasses(IEnumerable<Type> concreteClasses)
        => concreteClasses
            .Where(HasAnyFactMethods);

    public IEnumerable<MethodInfo> TestMethods(IEnumerable<MethodInfo> publicMethods)
        => publicMethods
            .Where(x => x.Has<FactAttribute>())
            .Shuffle();

    static bool HasAnyFactMethods(Type type)
        => type.GetMethods().Any(x => x.Has<FactAttribute>());
}