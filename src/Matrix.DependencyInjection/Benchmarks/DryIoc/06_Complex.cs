using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<IFirstService, FirstService>(Reuse.Singleton);
        container.Register<ISecondService, SecondService>(Reuse.Singleton);
        container.Register<IThirdService, ThirdService>(Reuse.Singleton);
        container.Register<SubObject1>(Reuse.Transient);
        container.Register<SubObject2>(Reuse.Transient);
        container.Register<SubObject3>(Reuse.Transient);
        container.Register<ComplexRoot1>(Reuse.Transient);
        container.Register<ComplexRoot2>(Reuse.Transient);
        container.Register<ComplexRoot3>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> DryIoc() =>
        new(
            _dryIoc.Resolve<ComplexRoot1>(),
            _dryIoc.Resolve<ComplexRoot2>(),
            _dryIoc.Resolve<ComplexRoot3>());
}
