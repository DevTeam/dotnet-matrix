// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Bind<IConditionalService>("1").To<ConditionalService1>()
            .Bind<IConditionalService>("2").To<ConditionalService2>()
            .Bind<IConditionalService>("3").To<ConditionalService3>()
            .Root<ConditionalRoot1>(nameof(Pure1))
            .Root<ConditionalRoot2>(nameof(Pure2))
            .Root<ConditionalRoot3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> PureDI()
    {
        var first = Pure1;
        var second = Pure2;
        var third = Pure3;
        Validate(LibraryCatalog.PureDi, first, second, third);
        return new(first, second, third);
    }
}
