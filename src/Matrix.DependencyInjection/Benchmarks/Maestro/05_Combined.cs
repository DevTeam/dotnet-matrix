using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add<ICombinedSingleton>().Type<CombinedSingleton>().Singleton();
            builder.Add<ICombinedTransient>().Type<CombinedTransient>().Transient();
            builder.Add<CombinedRoot1>().Self().Transient();
            builder.Add<CombinedRoot2>().Self().Transient();
            builder.Add<CombinedRoot3>().Self().Transient();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Maestro()
    {
        var first = _maestro.GetService<CombinedRoot1>();
        var second = _maestro.GetService<CombinedRoot2>();
        var third = _maestro.GetService<CombinedRoot3>();
        Validate(LibraryCatalog.Maestro, first, second, third);
        return new(first, second, third);
    }
}
