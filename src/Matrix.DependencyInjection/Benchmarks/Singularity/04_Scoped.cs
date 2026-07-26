using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private Container _singularity = null!;

    [GlobalSetup(Target = nameof(Singularity))]
    public void SetupSingularity() =>
        _singularity = new Container(builder =>
        {
            builder.Register<IScopedDependency, ScopedDependency>(c => c.With(Lifetimes.PerScope));
            builder.Register<ScopedRoot>(c => c.With(Lifetimes.Transient));
        });

    [GlobalCleanup(Target = nameof(Singularity))]
    public void CleanupSingularity() => _singularity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Singularity()
    {
        using var scope = _singularity.BeginScope();
        var first = scope.GetInstance<ScopedRoot>()!;
        var second = scope.GetInstance<ScopedRoot>()!;
        Validate(LibraryCatalog.Singularity, first, second);
        return new(first, second);
    }
}
