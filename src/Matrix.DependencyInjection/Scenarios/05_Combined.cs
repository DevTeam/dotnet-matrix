using System.Composition;
using Export = System.Composition.ExportAttribute;

namespace Matrix.DependencyInjection.Scenarios;

public interface ICombinedSingleton;
public interface ICombinedTransient;

[Export(typeof(ICombinedSingleton))]
[Shared]
public sealed class CombinedSingleton : ICombinedSingleton;

[Export(typeof(ICombinedTransient))]
public sealed class CombinedTransient : ICombinedTransient;

public interface ICombinedRoot
{
    ICombinedSingleton Singleton { get; }

    ICombinedTransient Transient { get; }
}

[Export]
[method: ImportingConstructor]
public sealed class CombinedRoot1(
    ICombinedSingleton singleton,
    ICombinedTransient transient) : ICombinedRoot
{
    public ICombinedSingleton Singleton { get; } = singleton;

    public ICombinedTransient Transient { get; } = transient;
}

[Export]
[method: ImportingConstructor]
public sealed class CombinedRoot2(
    ICombinedSingleton singleton,
    ICombinedTransient transient) : ICombinedRoot
{
    public ICombinedSingleton Singleton { get; } = singleton;

    public ICombinedTransient Transient { get; } = transient;
}

[Export]
[method: ImportingConstructor]
public sealed class CombinedRoot3(
    ICombinedSingleton singleton,
    ICombinedTransient transient) : ICombinedRoot
{
    public ICombinedSingleton Singleton { get; } = singleton;

    public ICombinedTransient Transient { get; } = transient;
}
