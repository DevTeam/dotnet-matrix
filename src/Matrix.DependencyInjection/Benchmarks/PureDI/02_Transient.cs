// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Bind<ITransient1>().To<Transient1>()
            .Bind<ITransient2>().To<Transient2>()
            .Bind<ITransient3>().To<Transient3>()
            .Root<ITransient1>(nameof(Pure1))
            .Root<ITransient2>(nameof(Pure2))
            .Root<ITransient3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> PureDI()
    {
        var first = Pure1;
        var second = Pure2;
        var third = Pure3;
        Validate(LibraryCatalog.PureDi, first);
        return new(first, second, third);
    }
}
