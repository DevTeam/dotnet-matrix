// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 PureDI()
    {
        var composition = new PureDiPrepareComposition();
        return composition.Root;
    }
}

public partial class PureDiPrepareComposition
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Singleton<Singleton1, Singleton2, Singleton3>()
            .Transient<Transient1, Transient2, Transient3>()
            .Singleton<FirstService, SecondService, ThirdService>()
            .Transient<SubObject1, SubObject2, SubObject3>()
            .Root<ISingleton1>(nameof(Root))
            .Root<ISingleton2>(nameof(Singleton2Root))
            .Root<ISingleton3>(nameof(Singleton3Root))
            .Root<ITransient1>(nameof(Transient1Root))
            .Root<ITransient2>(nameof(Transient2Root))
            .Root<ITransient3>(nameof(Transient3Root))
            .Root<ComplexRoot1>(nameof(ComplexRoot));
}

