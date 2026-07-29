// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> HandCoded()
    {
        using var scope = new HandCodedScopedScope();
        return new(scope.Root, scope.Root);
    }

    private sealed class HandCodedScopedScope : IDisposable
    {
        private ScopedDependency? _dependency;

        public ScopedRoot Root => new(_dependency ??= new ScopedDependency());

        public void Dispose() => _dependency?.Dispose();
    }
}
