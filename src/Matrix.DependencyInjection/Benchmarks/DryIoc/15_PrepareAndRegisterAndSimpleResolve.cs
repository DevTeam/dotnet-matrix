using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 DryIoc()
    {
        using var container = new Container();
        container.Register<ISingleton1, Singleton1>(Reuse.Singleton);
        container.Register<ISingleton2, Singleton2>(Reuse.Singleton);
        container.Register<ISingleton3, Singleton3>(Reuse.Singleton);
        container.Register<ITransient1, Transient1>(Reuse.Transient);
        container.Register<ITransient2, Transient2>(Reuse.Transient);
        container.Register<ITransient3, Transient3>(Reuse.Transient);
        container.Register<IFirstService, FirstService>(Reuse.Singleton);
        container.Register<ISecondService, SecondService>(Reuse.Singleton);
        container.Register<IThirdService, ThirdService>(Reuse.Singleton);
        container.Register<SubObject1>(Reuse.Transient);
        container.Register<SubObject2>(Reuse.Transient);
        container.Register<SubObject3>(Reuse.Transient);
        container.Register<ComplexRoot1>(Reuse.Transient);
        return container.Resolve<ISingleton1>();
    }
}
