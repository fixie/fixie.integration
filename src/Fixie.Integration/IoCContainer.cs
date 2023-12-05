namespace Fixie.Integration;

public class IoCContainer : IDisposable
{
    readonly Dictionary<Type, Type> typeMappings = new();
    readonly List<object> instances = new();

    public void Add(Type requestedType, Type concreteType)
    {
        typeMappings[requestedType] = concreteType;
    }

    public object Construct(Type type)
    {
        var constructor = type.GetConstructors().Single();

        var parameters = constructor.GetParameters();

        var arguments = parameters.Select(p => Resolve(p.ParameterType)).ToArray();

        var instance = Activator.CreateInstance(type, arguments);

        if (instance == null)
            throw new Exception("Could not construct instance of type " + type);

        return instance;
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