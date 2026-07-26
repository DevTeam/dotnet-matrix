using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register(typeof(IGenericService<>), typeof(GenericService<>), Reuse.Transient);
        container.Register(typeof(GenericRoot<>), Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> DryIoc() =>
        new(
            _dryIoc.Resolve<GenericRoot<int>>(),
            _dryIoc.Resolve<GenericRoot<float>>(),
            _dryIoc.Resolve<GenericRoot<object>>());
}
