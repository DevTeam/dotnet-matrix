using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private ExportProvider _vsMef = null!;

    [GlobalSetup(Target = nameof(VsMef))]
    public void SetupVsMef() =>
        _vsMef = VsMefHost.Create(
            typeof(FirstService),
            typeof(SecondService),
            typeof(ThirdService),
            typeof(SubObject1),
            typeof(SubObject2),
            typeof(SubObject3),
            typeof(ComplexRoot1),
            typeof(ComplexRoot2),
            typeof(ComplexRoot3));

    [GlobalCleanup(Target = nameof(VsMef))]
    public void CleanupVsMef() => _vsMef.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> VsMef() =>
        new(
            _vsMef.GetExportedValue<ComplexRoot1>(),
            _vsMef.GetExportedValue<ComplexRoot2>(),
            _vsMef.GetExportedValue<ComplexRoot3>());
}
