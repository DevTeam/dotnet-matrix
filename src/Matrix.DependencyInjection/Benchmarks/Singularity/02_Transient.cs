using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private Container _singularity = null!;

    [GlobalSetup(Target = nameof(Singularity))]
    public void SetupSingularity() =>
        _singularity = new Container(builder =>
        {
            builder.Register<ITransient1, Transient1>(c => c.With(Lifetimes.Transient));
            builder.Register<ITransient2, Transient2>(c => c.With(Lifetimes.Transient));
            builder.Register<ITransient3, Transient3>(c => c.With(Lifetimes.Transient));
        });

    [GlobalCleanup(Target = nameof(Singularity))]
    public void CleanupSingularity() => _singularity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Singularity()
    {
        var first = _singularity.GetInstance<ITransient1>()!;
        var second = _singularity.GetInstance<ITransient2>()!;
        var third = _singularity.GetInstance<ITransient3>()!;
        Validate(LibraryCatalog.Singularity, first);
        return new(first, second, third);
    }
}
