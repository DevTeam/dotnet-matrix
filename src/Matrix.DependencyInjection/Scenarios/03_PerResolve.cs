namespace Matrix.DependencyInjection.Scenarios;

public interface IPerResolveDependency;
public sealed class PerResolveDependency : IPerResolveDependency;

public sealed class PerResolveRoot(
    IPerResolveDependency first,
    IPerResolveDependency second)
{
    public IPerResolveDependency First { get; } = first;

    public IPerResolveDependency Second { get; } = second;
}
