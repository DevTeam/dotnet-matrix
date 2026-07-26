using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private CompositionHost _mef2 = null!;

    [GlobalSetup(Target = nameof(Mef2))]
    public void SetupMef2() =>
        _mef2 = new ContainerConfiguration()
            .WithParts(
                typeof(CombinedSingleton),
                typeof(CombinedTransient),
                typeof(CombinedRoot1),
                typeof(CombinedRoot2),
                typeof(CombinedRoot3))
            .CreateContainer();

    [GlobalCleanup(Target = nameof(Mef2))]
    public void CleanupMef2() => _mef2.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Mef2()
    {
        var first = _mef2.GetExport<CombinedRoot1>();
        var second = _mef2.GetExport<CombinedRoot2>();
        var third = _mef2.GetExport<CombinedRoot3>();
        Validate(LibraryCatalog.Mef2, first, second, third);
        return new(first, second, third);
    }
}
