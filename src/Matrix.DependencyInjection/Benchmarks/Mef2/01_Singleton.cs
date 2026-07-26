using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private CompositionHost _mef2 = null!;

    [GlobalSetup(Target = nameof(Mef2))]
    public void SetupMef2() =>
        _mef2 = new ContainerConfiguration()
            .WithParts(typeof(Singleton1), typeof(Singleton2), typeof(Singleton3))
            .CreateContainer();

    [GlobalCleanup(Target = nameof(Mef2))]
    public void CleanupMef2() => _mef2.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Mef2()
    {
        var first = _mef2.GetExport<ISingleton1>();
        var second = _mef2.GetExport<ISingleton2>();
        var third = _mef2.GetExport<ISingleton3>();
        Validate(LibraryCatalog.Mef2, first, second, third);
        return new(first, second, third);
    }
}
