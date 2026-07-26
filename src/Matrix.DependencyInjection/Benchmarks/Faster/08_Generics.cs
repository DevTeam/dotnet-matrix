using Faster.Ioc;
using FasterLifetime = Faster.Ioc.Contracts.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private Container _faster = null!;

    [GlobalSetup(Target = nameof(Faster))]
    public void SetupFaster()
    {
        var container = new Container();
        container.Register(
            typeof(IGenericService<>),
            typeof(GenericService<>),
            FasterLifetime.Transient);
        container.Register(typeof(GenericRoot<>), typeof(GenericRoot<>), FasterLifetime.Transient);
        _faster = container;
    }

    [GlobalCleanup(Target = nameof(Faster))]
    public void CleanupFaster() => _faster.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FasterIoc)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Faster() =>
        new(
            _faster.Resolve<GenericRoot<int>>(),
            _faster.Resolve<GenericRoot<float>>(),
            _faster.Resolve<GenericRoot<object>>());
}
