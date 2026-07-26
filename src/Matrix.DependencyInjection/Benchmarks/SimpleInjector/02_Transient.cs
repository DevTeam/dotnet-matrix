using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Register<ITransient1, Transient1>();
        container.Register<ITransient2, Transient2>();
        container.Register<ITransient3, Transient3>();
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> SimpleInjector()
    {
        var first = _simpleInjector.GetInstance<ITransient1>();
        var second = _simpleInjector.GetInstance<ITransient2>();
        var third = _simpleInjector.GetInstance<ITransient3>();
        Validate(LibraryCatalog.SimpleInjector, first);
        return new(first, second, third);
    }
}
