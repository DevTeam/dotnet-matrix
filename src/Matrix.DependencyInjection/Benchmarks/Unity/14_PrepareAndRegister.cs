using Unity;
using Unity.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Unity)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Unity()
    {
        using var container = new UnityContainer();
        container.RegisterType<ISingleton1, Singleton1>(new ContainerControlledLifetimeManager());
        container.RegisterType<ISingleton2, Singleton2>(new ContainerControlledLifetimeManager());
        container.RegisterType<ISingleton3, Singleton3>(new ContainerControlledLifetimeManager());
        container.RegisterType<ITransient1, Transient1>(new TransientLifetimeManager());
        container.RegisterType<ITransient2, Transient2>(new TransientLifetimeManager());
        container.RegisterType<ITransient3, Transient3>(new TransientLifetimeManager());
        container.RegisterType<IFirstService, FirstService>(new ContainerControlledLifetimeManager());
        container.RegisterType<ISecondService, SecondService>(new ContainerControlledLifetimeManager());
        container.RegisterType<IThirdService, ThirdService>(new ContainerControlledLifetimeManager());
        container.RegisterType<SubObject1>(new TransientLifetimeManager());
        container.RegisterType<SubObject2>(new TransientLifetimeManager());
        container.RegisterType<SubObject3>(new TransientLifetimeManager());
        container.RegisterType<ComplexRoot1>(new TransientLifetimeManager());
    }
}
