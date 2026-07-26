using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.RegisterSingleton<ICombinedSingleton, CombinedSingleton>();
        container.Register<ICombinedTransient, CombinedTransient>();
        container.Register<CombinedRoot1>();
        container.Register<CombinedRoot2>();
        container.Register<CombinedRoot3>();
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> SimpleInjector()
    {
        var first = _simpleInjector.GetInstance<CombinedRoot1>();
        var second = _simpleInjector.GetInstance<CombinedRoot2>();
        var third = _simpleInjector.GetInstance<CombinedRoot3>();
        Validate(LibraryCatalog.SimpleInjector, first, second, third);
        return new(first, second, third);
    }
}
