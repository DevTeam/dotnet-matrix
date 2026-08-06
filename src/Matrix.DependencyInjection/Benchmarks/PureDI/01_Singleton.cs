// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Singleton<Singleton1, Singleton2, Singleton3>()
            .Root<ISingleton1>(nameof(Pure1))
            .Root<ISingleton2>(nameof(Pure2))
            .Root<ISingleton3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> PureDI()
    {
        var first = Pure1;
        var second = Pure2;
        var third = Pure3;
        Validate(LibraryCatalog.PureDi, first, second, third);
        return new(first, second, third);
    }
}
