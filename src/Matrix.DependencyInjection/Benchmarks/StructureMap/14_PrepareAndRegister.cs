using StructureMap;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructureMap)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void StructureMap()
    {
        using var container = new Container(registry =>
        {
            registry.For<ISingleton1>().Singleton().Use<Singleton1>();
            registry.For<ISingleton2>().Singleton().Use<Singleton2>();
            registry.For<ISingleton3>().Singleton().Use<Singleton3>();
            registry.For<ITransient1>().AlwaysUnique().Use<Transient1>();
            registry.For<ITransient2>().AlwaysUnique().Use<Transient2>();
            registry.For<ITransient3>().AlwaysUnique().Use<Transient3>();
            registry.For<IFirstService>().Singleton().Use<FirstService>();
            registry.For<ISecondService>().Singleton().Use<SecondService>();
            registry.For<IThirdService>().Singleton().Use<ThirdService>();
            registry.For<SubObject1>().AlwaysUnique().Use<SubObject1>();
            registry.For<SubObject2>().AlwaysUnique().Use<SubObject2>();
            registry.For<SubObject3>().AlwaysUnique().Use<SubObject3>();
            registry.For<ComplexRoot1>().AlwaysUnique().Use<ComplexRoot1>();
        });
    }
}
