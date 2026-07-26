using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.Register(typeof(IGenericService<>), typeof(GenericService<>));
        container.Register(typeof(GenericRoot<>));
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Stashbox() =>
        new(
            _stashbox.Resolve<GenericRoot<int>>(),
            _stashbox.Resolve<GenericRoot<float>>(),
            _stashbox.Resolve<GenericRoot<object>>());
}
