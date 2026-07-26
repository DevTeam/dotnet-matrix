using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 SimpleInjector()
    {
        using var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.RegisterSingleton<ISingleton1, Singleton1>();
        container.RegisterSingleton<ISingleton2, Singleton2>();
        container.RegisterSingleton<ISingleton3, Singleton3>();
        container.Register<ITransient1, Transient1>();
        container.Register<ITransient2, Transient2>();
        container.Register<ITransient3, Transient3>();
        container.RegisterSingleton<IFirstService, FirstService>();
        container.RegisterSingleton<ISecondService, SecondService>();
        container.RegisterSingleton<IThirdService, ThirdService>();
        container.Register<SubObject1>();
        container.Register<SubObject2>();
        container.Register<SubObject3>();
        container.Register<ComplexRoot1>();
        return container.GetInstance<ISingleton1>();
    }
}
