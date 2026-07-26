using Faster.Ioc;
using FasterLifetime = Faster.Ioc.Contracts.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FasterIoc)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Faster()
    {
        using var container = new Container();
        container.Register<ISingleton1, Singleton1>(FasterLifetime.Singleton);
        container.Register<ISingleton2, Singleton2>(FasterLifetime.Singleton);
        container.Register<ISingleton3, Singleton3>(FasterLifetime.Singleton);
        container.Register<ITransient1, Transient1>(FasterLifetime.Transient);
        container.Register<ITransient2, Transient2>(FasterLifetime.Transient);
        container.Register<ITransient3, Transient3>(FasterLifetime.Transient);
        container.Register<IFirstService, FirstService>(FasterLifetime.Singleton);
        container.Register<ISecondService, SecondService>(FasterLifetime.Singleton);
        container.Register<IThirdService, ThirdService>(FasterLifetime.Singleton);
        container.Register<SubObject1>(FasterLifetime.Transient);
        container.Register<SubObject2>(FasterLifetime.Transient);
        container.Register<SubObject3>(FasterLifetime.Transient);
        container.Register<ComplexRoot1>(FasterLifetime.Transient);
    }
}
