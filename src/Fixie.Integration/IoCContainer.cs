namespace Fixie.Integration;

public class IoCContainer : IDisposable
{
    readonly Dictionary<Type, Type> typeMappings = new Dictionary<Type, Type>();
    readonly List<object> instances = new List<object>();

    public void Add(Type requestedType, Type concreteType)
    {
        typeMappings[requestedType] = concreteType;
    }

    public object Construct(Type type)
    {
        var constructor = type.GetConstructors().Single();

        var parameters = constructor.GetParameters();

        var arguments = parameters.Select(p => Resolve(p.ParameterType)).ToArray();

        return Activator.CreateInstance(type, arguments);
    }

    public object Resolve(Type type)
    {
        if (typeMappings.ContainsKey(type))
            type = typeMappings[type];

        var instance = Construct(type);

        instances.Add(instance);

        return instance;
    }

    public void Dispose()
    {
        foreach (var instance in instances)
            (instance as IDisposable)?.Dispose();
    }
}