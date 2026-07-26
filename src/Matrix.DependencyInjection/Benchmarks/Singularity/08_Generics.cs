using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private Container _singularity = null!;

    [GlobalSetup(Target = nameof(Singularity))]
    public void SetupSingularity() =>
        _singularity = new Container(builder =>
        {
            builder.Register(
                typeof(IGenericService<>),
                typeof(GenericService<>),
                c => c.With(Lifetimes.Transient));
            builder.Register(typeof(GenericRoot<>), c => c.With(Lifetimes.Transient));
        });

    [GlobalCleanup(Target = nameof(Singularity))]
    public void CleanupSingularity() => _singularity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Singularity() =>
        new(
            _singularity.GetInstance<GenericRoot<int>>()!,
            _singularity.GetInstance<GenericRoot<float>>()!,
            _singularity.GetInstance<GenericRoot<object>>()!);
}
