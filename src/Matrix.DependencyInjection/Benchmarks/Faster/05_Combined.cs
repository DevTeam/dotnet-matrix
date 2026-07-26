using Faster.Ioc;
using FasterLifetime = Faster.Ioc.Contracts.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private Container _faster = null!;

    [GlobalSetup(Target = nameof(Faster))]
    public void SetupFaster()
    {
        var container = new Container();
        container.Register<ICombinedSingleton, CombinedSingleton>(FasterLifetime.Singleton);
        container.Register<ICombinedTransient, CombinedTransient>(FasterLifetime.Transient);
        container.Register<CombinedRoot1>(FasterLifetime.Transient);
        container.Register<CombinedRoot2>(FasterLifetime.Transient);
        container.Register<CombinedRoot3>(FasterLifetime.Transient);
        _faster = container;
    }

    [GlobalCleanup(Target = nameof(Faster))]
    public void CleanupFaster() => _faster.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FasterIoc)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Faster()
    {
        var first = _faster.Resolve<CombinedRoot1>();
        var second = _faster.Resolve<CombinedRoot2>();
        var third = _faster.Resolve<CombinedRoot3>();
        Validate(LibraryCatalog.FasterIoc, first, second, third);
        return new(first, second, third);
    }
}
