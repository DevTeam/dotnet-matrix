using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.RegisterSingleton<ISingleton1, Singleton1>();
        container.RegisterSingleton<ISingleton2, Singleton2>();
        container.RegisterSingleton<ISingleton3, Singleton3>();
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> SimpleInjector()
    {
        var first = _simpleInjector.GetInstance<ISingleton1>();
        var second = _simpleInjector.GetInstance<ISingleton2>();
        var third = _simpleInjector.GetInstance<ISingleton3>();
        Validate(LibraryCatalog.SimpleInjector, first, second, third);
        return new(first, second, third);
    }
}
