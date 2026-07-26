using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For<ISingleton1>().Use<Singleton1>().Singleton();
            registry.For<ISingleton2>().Use<Singleton2>().Singleton();
            registry.For<ISingleton3>().Use<Singleton3>().Singleton();
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Lamar()
    {
        var first = _lamar.GetInstance<ISingleton1>();
        var second = _lamar.GetInstance<ISingleton2>();
        var third = _lamar.GetInstance<ISingleton3>();
        Validate(LibraryCatalog.Lamar, first, second, third);
        return new(first, second, third);
    }
}
