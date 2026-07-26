using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Enumerable
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Collection.Register<IPlugin>(typeof(Plugin1), typeof(Plugin2), typeof(Plugin3), typeof(Plugin4), typeof(Plugin5));
        container.Register<EnumerableRoot1>();
        container.Register<EnumerableRoot2>();
        container.Register<EnumerableRoot3>();
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<EnumerableRoot1, EnumerableRoot2, EnumerableRoot3> SimpleInjector()
    {
        var first = _simpleInjector.GetInstance<EnumerableRoot1>();
        var second = _simpleInjector.GetInstance<EnumerableRoot2>();
        var third = _simpleInjector.GetInstance<EnumerableRoot3>();
        Validate(LibraryCatalog.SimpleInjector, first, second, third);
        return new(first, second, third);
    }
}
