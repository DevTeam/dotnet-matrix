using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.RegisterSingleton<IFirstService, FirstService>();
        container.RegisterSingleton<ISecondService, SecondService>();
        container.RegisterSingleton<IThirdService, ThirdService>();
        container.Register<SubObject1>();
        container.Register<SubObject2>();
        container.Register<SubObject3>();
        container.Register<ComplexRoot1>();
        container.Register<ComplexRoot2>();
        container.Register<ComplexRoot3>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Stashbox() =>
        new(
            _stashbox.Resolve<ComplexRoot1>(),
            _stashbox.Resolve<ComplexRoot2>(),
            _stashbox.Resolve<ComplexRoot3>());
}
