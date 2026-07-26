// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Bind<IPlugin>(Tag.Unique).To<Plugin1>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin2>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin3>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin4>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin5>()
            .Root<ArrayRoot1>(nameof(Pure1))
            .Root<ArrayRoot2>(nameof(Pure2))
            .Root<ArrayRoot3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> PureDI()
    {
        var first = Pure1;
        var second = Pure2;
        var third = Pure3;
        Validate(LibraryCatalog.PureDi, first, second, third);
        return new(first, second, third);
    }
}
