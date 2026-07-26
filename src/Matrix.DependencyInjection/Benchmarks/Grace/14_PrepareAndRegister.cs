using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Grace()
    {
        using var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<Singleton1>().As<ISingleton1>().Lifestyle.Singleton();
            block.Export<Singleton2>().As<ISingleton2>().Lifestyle.Singleton();
            block.Export<Singleton3>().As<ISingleton3>().Lifestyle.Singleton();
            block.Export<Transient1>().As<ITransient1>();
            block.Export<Transient2>().As<ITransient2>();
            block.Export<Transient3>().As<ITransient3>();
            block.Export<FirstService>().As<IFirstService>().Lifestyle.Singleton();
            block.Export<SecondService>().As<ISecondService>().Lifestyle.Singleton();
            block.Export<ThirdService>().As<IThirdService>().Lifestyle.Singleton();
            block.Export<SubObject1>();
            block.Export<SubObject2>();
            block.Export<SubObject3>();
            block.Export<ComplexRoot1>();
        });
    }
}
