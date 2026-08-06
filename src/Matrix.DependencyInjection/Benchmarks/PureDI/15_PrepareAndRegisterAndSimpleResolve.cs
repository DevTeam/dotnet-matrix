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
            .Bind<ISingleton1>().As(Lifetime.Singleton).To<Singleton1>()
            .Bind<ISingleton2>().As(Lifetime.Singleton).To<Singleton2>()
            .Bind<ISingleton3>().As(Lifetime.Singleton).To<Singleton3>()
            .Bind<ITransient1>().To<Transient1>()
            .Bind<ITransient2>().To<Transient2>()
            .Bind<ITransient3>().To<Transient3>()
            .Bind<IFirstService>().As(Lifetime.Singleton).To<FirstService>()
            .Bind<ISecondService>().As(Lifetime.Singleton).To<SecondService>()
            .Bind<IThirdService>().As(Lifetime.Singleton).To<ThirdService>()
            .Bind().To<SubObject1>()
            .Bind().To<SubObject2>()
            .Bind().To<SubObject3>()
            .Root<ISingleton1>(nameof(Root))
            .Root<ISingleton2>(nameof(Singleton2Root))
            .Root<ISingleton3>(nameof(Singleton3Root))
            .Root<ITransient1>(nameof(Transient1Root))
            .Root<ITransient2>(nameof(Transient2Root))
            .Root<ITransient3>(nameof(Transient3Root))
            .Root<ComplexRoot1>(nameof(ComplexRoot));
}

