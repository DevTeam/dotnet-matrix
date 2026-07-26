// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Bind<ICombinedSingleton>().As(Lifetime.Singleton).To<CombinedSingleton>()
            .Bind<ICombinedTransient>().To<CombinedTransient>()
            .Root<CombinedRoot1>(nameof(Pure1))
            .Root<CombinedRoot2>(nameof(Pure2))
            .Root<CombinedRoot3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> PureDI()
    {
        var first = Pure1;
        var second = Pure2;
        var third = Pure3;
        Validate(LibraryCatalog.PureDi, first, second, third);
        return new(first, second, third);
    }
}
