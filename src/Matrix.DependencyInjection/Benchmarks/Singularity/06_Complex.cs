using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private Container _singularity = null!;

    [GlobalSetup(Target = nameof(Singularity))]
    public void SetupSingularity() =>
        _singularity = new Container(builder =>
        {
            builder.Register<IFirstService, FirstService>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ISecondService, SecondService>(c => c.With(Lifetimes.PerContainer));
            builder.Register<IThirdService, ThirdService>(c => c.With(Lifetimes.PerContainer));
            builder.Register<SubObject1>(c => c.With(Lifetimes.Transient));
            builder.Register<SubObject2>(c => c.With(Lifetimes.Transient));
            builder.Register<SubObject3>(c => c.With(Lifetimes.Transient));
            builder.Register<ComplexRoot1>(c => c.With(Lifetimes.Transient));
            builder.Register<ComplexRoot2>(c => c.With(Lifetimes.Transient));
            builder.Register<ComplexRoot3>(c => c.With(Lifetimes.Transient));
        });

    [GlobalCleanup(Target = nameof(Singularity))]
    public void CleanupSingularity() => _singularity.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Singularity() =>
        new(
            _singularity.GetInstance<ComplexRoot1>()!,
            _singularity.GetInstance<ComplexRoot2>()!,
            _singularity.GetInstance<ComplexRoot3>()!);
}
