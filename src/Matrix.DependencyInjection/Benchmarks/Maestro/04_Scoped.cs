using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add<IScopedDependency>().Type<ScopedDependency>().Scoped();
            builder.Add<ScopedRoot>().Self().Transient();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Maestro()
    {
        using var scope = _maestro.CreateScope();
        var first = scope.GetService<ScopedRoot>();
        var second = scope.GetService<ScopedRoot>();
        Validate(LibraryCatalog.Maestro, first, second);
        return new(first, second);
    }
}
