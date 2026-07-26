using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<IScopedDependency, ScopedDependency>(new PerScopeLifetime());
        container.Register<ScopedRoot>();
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> LightInject()
    {
        using var scope = _lightInject.BeginScope();
        var first = scope.GetInstance<ScopedRoot>();
        var second = scope.GetInstance<ScopedRoot>();
        Validate(LibraryCatalog.LightInject, first, second);
        return new(first, second);
    }
}
