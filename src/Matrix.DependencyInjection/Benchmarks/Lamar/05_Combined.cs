using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For<ICombinedSingleton>().Use<CombinedSingleton>().Singleton();
            registry.For<ICombinedTransient>().Use<CombinedTransient>().Transient();
            registry.For<CombinedRoot1>().Use<CombinedRoot1>().Transient();
            registry.For<CombinedRoot2>().Use<CombinedRoot2>().Transient();
            registry.For<CombinedRoot3>().Use<CombinedRoot3>().Transient();
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Lamar()
    {
        var first = _lamar.GetInstance<CombinedRoot1>();
        var second = _lamar.GetInstance<CombinedRoot2>();
        var third = _lamar.GetInstance<CombinedRoot3>();
        Validate(LibraryCatalog.Lamar, first, second, third);
        return new(first, second, third);
    }
}
