// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Enumerable
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Bind<IPlugin>(Tag.Unique).To<Plugin1>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin2>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin3>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin4>()
            .Bind<IPlugin>(Tag.Unique).To<Plugin5>()
            .Bind(Tag.OnConstructorArg<EnumerableRoot1>("plugins"), Tag.OnConstructorArg<EnumerableRoot2>("plugins"), Tag.OnConstructorArg<EnumerableRoot3>("plugins"))
                .As(Lifetime.Singleton)
                .To(IEnumerable<IPlugin> (IEnumerable<IPlugin> plugins) => plugins)
            .Root<EnumerableRoot1>(nameof(Pure1))
            .Root<EnumerableRoot2>(nameof(Pure2))
            .Root<EnumerableRoot3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<EnumerableRoot1, EnumerableRoot2, EnumerableRoot3> PureDI()
    {
        var first = Pure1;
        var second = Pure2;
        var third = Pure3;
        Validate(LibraryCatalog.PureDi, first, second, third);
        return new(first, second, third);
    }
}
