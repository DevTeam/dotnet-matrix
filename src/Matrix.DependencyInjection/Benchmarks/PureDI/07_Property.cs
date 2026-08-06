// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Transient<PropertyServiceA, PropertyServiceB, PropertyServiceC>()
            .Root<PropertyRoot1>(nameof(Pure1))
            .Root<PropertyRoot2>(nameof(Pure2))
            .Root<PropertyRoot3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> PureDI()
    {
        var first = Pure1;
        var second = Pure2;
        var third = Pure3;
        Validate(LibraryCatalog.PureDi, first, second, third);
        return new(first, second, third);
    }
}
