using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private Container _singularity = null!;

    [GlobalSetup(Target = nameof(Singularity))]
    public void SetupSingularity() =>
        _singularity = new Container(builder =>
        {
            builder.Register<ISingleton1, Singleton1>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ISingleton2, Singleton2>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ISingleton3, Singleton3>(c => c.With(Lifetimes.PerContainer));
        });

    [GlobalCleanup(Target = nameof(Singularity))]
    public void CleanupSingularity() => _singularity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Singularity()
    {
        var first = _singularity.GetInstance<ISingleton1>()!;
        var second = _singularity.GetInstance<ISingleton2>()!;
        var third = _singularity.GetInstance<ISingleton3>()!;
        Validate(LibraryCatalog.Singularity, first, second, third);
        return new(first, second, third);
    }
}
