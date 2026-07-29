// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> HandCoded()
    {
        return new(
            HandCodedSingletons.First,
            HandCodedSingletons.Second,
            HandCodedSingletons.Third);
    }

    private static class HandCodedSingletons
    {
        public static readonly ISingleton1 First = new Singleton1();
        public static readonly ISingleton2 Second = new Singleton2();
        public static readonly ISingleton3 Third = new Singleton3();
    }
}
