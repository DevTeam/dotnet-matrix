// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.VsMef)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void VsMef()
    {
        using var provider = VsMefHost.Create(
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
            typeof(ComplexRoot1));
    }
}
