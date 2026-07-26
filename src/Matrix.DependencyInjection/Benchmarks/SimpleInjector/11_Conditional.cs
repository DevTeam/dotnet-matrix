using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Register<ConditionalRoot1>();
        container.Register<ConditionalRoot2>();
        container.Register<ConditionalRoot3>();
        container.RegisterConditional<IConditionalService, ConditionalService1>(
            context => context.Consumer.ImplementationType == typeof(ConditionalRoot1));
        container.RegisterConditional<IConditionalService, ConditionalService2>(
            context => context.Consumer.ImplementationType == typeof(ConditionalRoot2));
        container.RegisterConditional<IConditionalService, ConditionalService3>(
            context => context.Consumer.ImplementationType == typeof(ConditionalRoot3));
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> SimpleInjector()
    {
        var first = _simpleInjector.GetInstance<ConditionalRoot1>();
        var second = _simpleInjector.GetInstance<ConditionalRoot2>();
        var third = _simpleInjector.GetInstance<ConditionalRoot3>();
        Validate(LibraryCatalog.SimpleInjector, first, second, third);
        return new(first, second, third);
    }
}
