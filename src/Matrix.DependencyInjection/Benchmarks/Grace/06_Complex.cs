using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<FirstService>().As<IFirstService>().Lifestyle.Singleton();
            block.Export<SecondService>().As<ISecondService>().Lifestyle.Singleton();
            block.Export<ThirdService>().As<IThirdService>().Lifestyle.Singleton();
            block.Export<SubObject1>();
            block.Export<SubObject2>();
            block.Export<SubObject3>();
            block.Export<ComplexRoot1>();
            block.Export<ComplexRoot2>();
            block.Export<ComplexRoot3>();
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Grace() =>
        new(
            _grace.Locate<ComplexRoot1>(),
            _grace.Locate<ComplexRoot2>(),
            _grace.Locate<ComplexRoot3>());
}
