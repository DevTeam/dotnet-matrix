using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private StandardKernel _ninject = null!;
    private NinjectScope _ninjectScope = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<IScopedDependency>().To<ScopedDependency>().InScope(_ => _ninjectScope);
        kernel.Bind<ScopedRoot>().ToSelf().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Ninject()
    {
        _ninjectScope = new NinjectScope();
        var first = _ninject.Get<ScopedRoot>();
        var second = _ninject.Get<ScopedRoot>();
        Validate(LibraryCatalog.Ninject, first, second);
        _ninject.Release(_ninjectScope);
        return new(first, second);
    }

    // Ninject scopes are keyed by an arbitrary scope object supplied through InScope.
    private sealed class NinjectScope;
}
