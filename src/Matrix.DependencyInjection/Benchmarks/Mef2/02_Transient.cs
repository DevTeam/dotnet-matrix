using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private CompositionHost _mef2 = null!;

    [GlobalSetup(Target = nameof(Mef2))]
    public void SetupMef2() =>
        _mef2 = new ContainerConfiguration()
            .WithParts(typeof(Transient1), typeof(Transient2), typeof(Transient3))
            .CreateContainer();

    [GlobalCleanup(Target = nameof(Mef2))]
    public void CleanupMef2() => _mef2.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Mef2()
    {
        var first = _mef2.GetExport<ITransient1>();
        var second = _mef2.GetExport<ITransient2>();
        var third = _mef2.GetExport<ITransient3>();
        Validate(LibraryCatalog.Mef2, first);
        return new(first, second, third);
    }
}
