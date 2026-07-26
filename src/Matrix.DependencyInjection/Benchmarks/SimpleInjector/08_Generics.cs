using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Register(typeof(IGenericService<>), typeof(GenericService<>));
        container.Register(typeof(GenericRoot<>), typeof(GenericRoot<>));
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> SimpleInjector() =>
        new(
            _simpleInjector.GetInstance<GenericRoot<int>>(),
            _simpleInjector.GetInstance<GenericRoot<float>>(),
            _simpleInjector.GetInstance<GenericRoot<object>>());
}
