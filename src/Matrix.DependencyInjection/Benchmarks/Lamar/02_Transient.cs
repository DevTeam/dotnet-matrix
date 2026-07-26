using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For<ITransient1>().Use<Transient1>().Transient();
            registry.For<ITransient2>().Use<Transient2>().Transient();
            registry.For<ITransient3>().Use<Transient3>().Transient();
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Lamar()
    {
        var first = _lamar.GetInstance<ITransient1>();
        var second = _lamar.GetInstance<ITransient2>();
        var third = _lamar.GetInstance<ITransient3>();
        Validate(LibraryCatalog.Lamar, first);
        return new(first, second, third);
    }
}
