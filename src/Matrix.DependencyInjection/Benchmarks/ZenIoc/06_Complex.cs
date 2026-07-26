using ZenIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private IIocContainer _zenIoc = null!;

    [GlobalSetup(Target = nameof(ZenIoc))]
    public void SetupZenIoc()
    {
        IIocContainer container = new IocContainer();
        container.Register<IFirstService, FirstService>().SingleInstance();
        container.Register<ISecondService, SecondService>().SingleInstance();
        container.Register<IThirdService, ThirdService>().SingleInstance();
        container.Register<SubObject1>();
        container.Register<SubObject2>();
        container.Register<SubObject3>();
        container.Register<ComplexRoot1>();
        container.Register<ComplexRoot2>();
        container.Register<ComplexRoot3>();
        container.Compile();
        _zenIoc = container;
    }

    [GlobalCleanup(Target = nameof(ZenIoc))]
    public void CleanupZenIoc() => _zenIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZenIoc)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> ZenIoc() =>
        new(
            _zenIoc.Resolve<ComplexRoot1>(),
            _zenIoc.Resolve<ComplexRoot2>(),
            _zenIoc.Resolve<ComplexRoot3>());
}
