using SimpleInjector;
using SimpleInjector.Lifestyles;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
        container.Register<IScopedDependency, ScopedDependency>(Lifestyle.Scoped);
        container.Register<ScopedRoot>();
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> SimpleInjector()
    {
        using var scope = ThreadScopedLifestyle.BeginScope(_simpleInjector);
        var first = _simpleInjector.GetInstance<ScopedRoot>();
        var second = _simpleInjector.GetInstance<ScopedRoot>();
        Validate(LibraryCatalog.SimpleInjector, first, second);
        return new(first, second);
    }
}
