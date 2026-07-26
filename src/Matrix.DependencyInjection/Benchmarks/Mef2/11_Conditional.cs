using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private CompositionHost _mef2 = null!;

    [GlobalSetup(Target = nameof(Mef2))]
    public void SetupMef2() =>
        _mef2 = new ContainerConfiguration()
            .WithParts(
                typeof(ConditionalService1),
                typeof(ConditionalService2),
                typeof(ConditionalService3),
                typeof(ConditionalRoot1),
                typeof(ConditionalRoot2),
                typeof(ConditionalRoot3))
            .CreateContainer();

    [GlobalCleanup(Target = nameof(Mef2))]
    public void CleanupMef2() => _mef2.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Mef2()
    {
        var first = _mef2.GetExport<ConditionalRoot1>();
        var second = _mef2.GetExport<ConditionalRoot2>();
        var third = _mef2.GetExport<ConditionalRoot3>();
        Validate(LibraryCatalog.Mef2, first, second, third);
        return new(first, second, third);
    }
}
