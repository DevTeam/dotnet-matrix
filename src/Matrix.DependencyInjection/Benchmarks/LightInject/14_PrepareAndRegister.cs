using LightInject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void LightInject()
    {
        using var container = new ServiceContainer();
        container.Register<ISingleton1, Singleton1>(new PerContainerLifetime());
        container.Register<ISingleton2, Singleton2>(new PerContainerLifetime());
        container.Register<ISingleton3, Singleton3>(new PerContainerLifetime());
        container.Register<ITransient1, Transient1>();
        container.Register<ITransient2, Transient2>();
        container.Register<ITransient3, Transient3>();
        container.Register<IFirstService, FirstService>(new PerContainerLifetime());
        container.Register<ISecondService, SecondService>(new PerContainerLifetime());
        container.Register<IThirdService, ThirdService>(new PerContainerLifetime());
        container.Register<SubObject1>();
        container.Register<SubObject2>();
        container.Register<SubObject3>();
        container.Register<ComplexRoot1>();
    }
}
