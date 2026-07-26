using Faster.Ioc;
using FasterLifetime = Faster.Ioc.Contracts.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private Container _faster = null!;

    [GlobalSetup(Target = nameof(Faster))]
    public void SetupFaster()
    {
        var container = new Container();
        container.Register<IScopedDependency, ScopedDependency>(FasterLifetime.Scoped);
        container.Register<ScopedRoot>(FasterLifetime.Transient);
        _faster = container;
    }

    [GlobalCleanup(Target = nameof(Faster))]
    public void CleanupFaster() => _faster.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FasterIoc)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Faster()
    {
        using var scope = _faster.CreateScope();
        var first = (ScopedRoot)scope.ServiceProvider.GetService(typeof(ScopedRoot))!;
        var second = (ScopedRoot)scope.ServiceProvider.GetService(typeof(ScopedRoot))!;
        Validate(LibraryCatalog.FasterIoc, first, second);
        return new(first, second);
    }
}
