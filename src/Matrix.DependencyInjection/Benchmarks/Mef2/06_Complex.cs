using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private CompositionHost _mef2 = null!;

    [GlobalSetup(Target = nameof(Mef2))]
    public void SetupMef2() =>
        _mef2 = new ContainerConfiguration()
            .WithParts(
                typeof(FirstService),
                typeof(SecondService),
                typeof(ThirdService),
                typeof(SubObject1),
                typeof(SubObject2),
                typeof(SubObject3),
                typeof(ComplexRoot1),
                typeof(ComplexRoot2),
                typeof(ComplexRoot3))
            .CreateContainer();

    [GlobalCleanup(Target = nameof(Mef2))]
    public void CleanupMef2() => _mef2.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Mef2() =>
        new(
            _mef2.GetExport<ComplexRoot1>(),
            _mef2.GetExport<ComplexRoot2>(),
            _mef2.GetExport<ComplexRoot3>());
}
