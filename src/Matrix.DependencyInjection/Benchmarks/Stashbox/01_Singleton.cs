using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.RegisterSingleton<ISingleton1, Singleton1>();
        container.RegisterSingleton<ISingleton2, Singleton2>();
        container.RegisterSingleton<ISingleton3, Singleton3>();
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Stashbox()
    {
        var first = _stashbox.Resolve<ISingleton1>();
        var second = _stashbox.Resolve<ISingleton2>();
        var third = _stashbox.Resolve<ISingleton3>();
        Validate(LibraryCatalog.Stashbox, first, second, third);
        return new(first, second, third);
    }
}
