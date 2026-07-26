using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private Container _singularity = null!;

    [GlobalSetup(Target = nameof(Singularity))]
    public void SetupSingularity() =>
        _singularity = new Container(builder =>
        {
            builder.Register(
                typeof(IPlugin),
                [typeof(Plugin1), typeof(Plugin2), typeof(Plugin3), typeof(Plugin4), typeof(Plugin5)],
                c => c.With(Lifetimes.Transient));
            builder.Register<ArrayRoot1>(c => c.With(Lifetimes.Transient));
            builder.Register<ArrayRoot2>(c => c.With(Lifetimes.Transient));
            builder.Register<ArrayRoot3>(c => c.With(Lifetimes.Transient));
        });

    [GlobalCleanup(Target = nameof(Singularity))]
    public void CleanupSingularity() => _singularity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Singularity()
    {
        var first = _singularity.GetInstance<ArrayRoot1>()!;
        var second = _singularity.GetInstance<ArrayRoot2>()!;
        var third = _singularity.GetInstance<ArrayRoot3>()!;
        Validate(LibraryCatalog.Singularity, first, second, third);
        return new(first, second, third);
    }
}
