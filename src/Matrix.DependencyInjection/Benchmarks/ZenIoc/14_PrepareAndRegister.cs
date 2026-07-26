using ZenIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZenIoc)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void ZenIoc()
    {
        using IIocContainer container = new IocContainer();
        container.Register<ISingleton1, Singleton1>().SingleInstance();
        container.Register<ISingleton2, Singleton2>().SingleInstance();
        container.Register<ISingleton3, Singleton3>().SingleInstance();
        container.Register<ITransient1, Transient1>();
        container.Register<ITransient2, Transient2>();
        container.Register<ITransient3, Transient3>();
        container.Register<IFirstService, FirstService>().SingleInstance();
        container.Register<ISecondService, SecondService>().SingleInstance();
        container.Register<IThirdService, ThirdService>().SingleInstance();
        container.Register<SubObject1>();
        container.Register<SubObject2>();
        container.Register<SubObject3>();
        container.Register<ComplexRoot1>();
    }
}
