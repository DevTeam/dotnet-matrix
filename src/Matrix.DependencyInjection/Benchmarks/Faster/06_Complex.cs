using Faster.Ioc;
using FasterLifetime = Faster.Ioc.Contracts.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private Container _faster = null!;

    [GlobalSetup(Target = nameof(Faster))]
    public void SetupFaster()
    {
        var container = new Container();
        container.Register<IFirstService, FirstService>(FasterLifetime.Singleton);
        container.Register<ISecondService, SecondService>(FasterLifetime.Singleton);
        container.Register<IThirdService, ThirdService>(FasterLifetime.Singleton);
        container.Register<SubObject1>(FasterLifetime.Transient);
        container.Register<SubObject2>(FasterLifetime.Transient);
        container.Register<SubObject3>(FasterLifetime.Transient);
        container.Register<ComplexRoot1>(FasterLifetime.Transient);
        container.Register<ComplexRoot2>(FasterLifetime.Transient);
        container.Register<ComplexRoot3>(FasterLifetime.Transient);
        _faster = container;
    }

    [GlobalCleanup(Target = nameof(Faster))]
    public void CleanupFaster() => _faster.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FasterIoc)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Faster() =>
        new(
            _faster.Resolve<ComplexRoot1>(),
            _faster.Resolve<ComplexRoot2>(),
            _faster.Resolve<ComplexRoot3>());
}
