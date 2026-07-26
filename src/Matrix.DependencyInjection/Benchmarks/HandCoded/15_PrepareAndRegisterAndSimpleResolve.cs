// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded, true)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 HandCoded() => new Singleton1();
}
