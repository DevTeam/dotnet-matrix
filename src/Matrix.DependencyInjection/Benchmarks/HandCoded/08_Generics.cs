// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded, true)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> HandCoded()
    {
        return new(
            new GenericRoot<int>(new GenericService<int>()),
            new GenericRoot<float>(new GenericService<float>()),
            new GenericRoot<object>(new GenericService<object>()));
    }
}
