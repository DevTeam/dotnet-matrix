using Faster.Ioc;
using FasterLifetime = Faster.Ioc.Contracts.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private Container _faster = null!;

    [GlobalSetup(Target = nameof(Faster))]
    public void SetupFaster()
    {
        var container = new Container();
        container.Register<ITransient1, Transient1>(FasterLifetime.Transient);
        container.Register<ITransient2, Transient2>(FasterLifetime.Transient);
        container.Register<ITransient3, Transient3>(FasterLifetime.Transient);
        _faster = container;
    }

    [GlobalCleanup(Target = nameof(Faster))]
    public void CleanupFaster() => _faster.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FasterIoc)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Faster()
    {
        var first = _faster.Resolve<ITransient1>();
        var second = _faster.Resolve<ITransient2>();
        var third = _faster.Resolve<ITransient3>();
        Validate(LibraryCatalog.FasterIoc, first);
        return new(first, second, third);
    }
}
