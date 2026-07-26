using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 Lamar()
    {
        using var container = Container.For(registry =>
        {
            registry.For<ISingleton1>().Use<Singleton1>().Singleton();
            registry.For<ISingleton2>().Use<Singleton2>().Singleton();
            registry.For<ISingleton3>().Use<Singleton3>().Singleton();
            registry.For<ITransient1>().Use<Transient1>().Transient();
            registry.For<ITransient2>().Use<Transient2>().Transient();
            registry.For<ITransient3>().Use<Transient3>().Transient();
            registry.For<IFirstService>().Use<FirstService>().Singleton();
            registry.For<ISecondService>().Use<SecondService>().Singleton();
            registry.For<IThirdService>().Use<ThirdService>().Singleton();
            registry.For<SubObject1>().Use<SubObject1>().Transient();
            registry.For<SubObject2>().Use<SubObject2>().Transient();
            registry.For<SubObject3>().Use<SubObject3>().Transient();
            registry.For<ComplexRoot1>().Use<ComplexRoot1>().Transient();
        });
        return container.GetInstance<ISingleton1>();
    }
}
