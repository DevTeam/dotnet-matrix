using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For<IScopedDependency>().Use<ScopedDependency>().Scoped();
            registry.For<ScopedRoot>().Use<ScopedRoot>().Transient();
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Lamar()
    {
        using var scope = _lamar.GetNestedContainer();
        var first = scope.GetInstance<ScopedRoot>();
        var second = scope.GetInstance<ScopedRoot>();
        Validate(LibraryCatalog.Lamar, first, second);
        return new BenchmarkRoots<ScopedRoot, ScopedRoot>(first, second);
    }
}
