using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For<IPlugin>().Add<Plugin1>().Transient();
            registry.For<IPlugin>().Add<Plugin2>().Transient();
            registry.For<IPlugin>().Add<Plugin3>().Transient();
            registry.For<IPlugin>().Add<Plugin4>().Transient();
            registry.For<IPlugin>().Add<Plugin5>().Transient();
            registry.For<ArrayRoot1>().Use<ArrayRoot1>().Transient();
            registry.For<ArrayRoot2>().Use<ArrayRoot2>().Transient();
            registry.For<ArrayRoot3>().Use<ArrayRoot3>().Transient();
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Lamar()
    {
        var first = _lamar.GetInstance<ArrayRoot1>();
        var second = _lamar.GetInstance<ArrayRoot2>();
        var third = _lamar.GetInstance<ArrayRoot3>();
        Validate(LibraryCatalog.Lamar, first, second, third);
        return new(first, second, third);
    }
}
