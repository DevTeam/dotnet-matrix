using Spectre.Console.Cli;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Resolves the one command instance a scenario holds, so the settings the binder produced
/// can be read after the run. Every other service the framework asks for is created
/// normally, and an empty sequence is returned for the interceptor and help-provider
/// enumerations the executor requests.
/// </summary>
internal sealed class SpectreRegistrar : ITypeRegistrar, ITypeResolver
{
    private readonly object _instance;

    public SpectreRegistrar(object instance) => _instance = instance;

    public ITypeResolver Build() => this;

    public object? Resolve(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        if (type.IsInstanceOfType(_instance))
        {
            return _instance;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return Array.CreateInstance(type.GetGenericArguments()[0], 0);
        }

        return Activator.CreateInstance(type);
    }

    public void Register(Type service, Type implementation)
    {
    }

    public void RegisterInstance(Type service, object implementation)
    {
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
    }
}
