using System.Composition.Hosting;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mef2)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Mef2()
    {
        using var container = new ContainerConfiguration()
            .WithParts(
                typeof(Singleton1),
                typeof(Singleton2),
                typeof(Singleton3),
                typeof(Transient1),
                typeof(Transient2),
                typeof(Transient3),
                typeof(FirstService),
                typeof(SecondService),
                typeof(ThirdService),
                typeof(SubObject1),
                typeof(SubObject2),
                typeof(SubObject3),
                typeof(ComplexRoot1))
            .CreateContainer();
    }
}
