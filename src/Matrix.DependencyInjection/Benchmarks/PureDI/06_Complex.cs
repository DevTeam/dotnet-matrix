// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Bind<IFirstService>().As(Lifetime.Singleton).To<FirstService>()
            .Bind<ISecondService>().As(Lifetime.Singleton).To<SecondService>()
            .Bind<IThirdService>().As(Lifetime.Singleton).To<ThirdService>()
            .Bind().To<SubObject1>()
            .Bind().To<SubObject2>()
            .Bind().To<SubObject3>()
            .Root<ComplexRoot1>(nameof(Pure1))
            .Root<ComplexRoot2>(nameof(Pure2))
            .Root<ComplexRoot3>(nameof(Pure3));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> PureDI()
    {
        return new(Pure1, Pure2, Pure3);
    }
}
