using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private CompositionHost _mef2 = null!;

    [GlobalSetup(Target = nameof(Mef2))]
    public void SetupMef2() =>
        _mef2 = new ContainerConfiguration()
            .WithParts(
                typeof(Plugin1),
                typeof(Plugin2),
                typeof(Plugin3),
                typeof(Plugin4),
                typeof(Plugin5),
                typeof(ArrayRoot1),
                typeof(ArrayRoot2),
                typeof(ArrayRoot3))
            .CreateContainer();

    [GlobalCleanup(Target = nameof(Mef2))]
    public void CleanupMef2() => _mef2.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Mef2()
    {
        var first = _mef2.GetExport<ArrayRoot1>();
        var second = _mef2.GetExport<ArrayRoot2>();
        var third = _mef2.GetExport<ArrayRoot3>();
        Validate(LibraryCatalog.Mef2, first, second, third);
        return new(first, second, third);
    }
}
