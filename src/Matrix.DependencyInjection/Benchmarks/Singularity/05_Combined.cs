using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private Container _singularity = null!;

    [GlobalSetup(Target = nameof(Singularity))]
    public void SetupSingularity() =>
        _singularity = new Container(builder =>
        {
            builder.Register<ICombinedSingleton, CombinedSingleton>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ICombinedTransient, CombinedTransient>(c => c.With(Lifetimes.Transient));
            builder.Register<CombinedRoot1>(c => c.With(Lifetimes.Transient));
            builder.Register<CombinedRoot2>(c => c.With(Lifetimes.Transient));
            builder.Register<CombinedRoot3>(c => c.With(Lifetimes.Transient));
        });

    [GlobalCleanup(Target = nameof(Singularity))]
    public void CleanupSingularity() => _singularity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Singularity()
    {
        var first = _singularity.GetInstance<CombinedRoot1>()!;
        var second = _singularity.GetInstance<CombinedRoot2>()!;
        var third = _singularity.GetInstance<CombinedRoot3>()!;
        Validate(LibraryCatalog.Singularity, first, second, third);
        return new(first, second, third);
    }
}
