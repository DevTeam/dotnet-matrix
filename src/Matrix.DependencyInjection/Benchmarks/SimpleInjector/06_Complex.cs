using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.RegisterSingleton<IFirstService, FirstService>();
        container.RegisterSingleton<ISecondService, SecondService>();
        container.RegisterSingleton<IThirdService, ThirdService>();
        container.Register<SubObject1>();
        container.Register<SubObject2>();
        container.Register<SubObject3>();
        container.Register<ComplexRoot1>();
        container.Register<ComplexRoot2>();
        container.Register<ComplexRoot3>();
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> SimpleInjector() =>
        new(
            _simpleInjector.GetInstance<ComplexRoot1>(),
            _simpleInjector.GetInstance<ComplexRoot2>(),
            _simpleInjector.GetInstance<ComplexRoot3>());
}
