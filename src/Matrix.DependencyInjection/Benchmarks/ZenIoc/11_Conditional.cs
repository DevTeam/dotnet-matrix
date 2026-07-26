using ZenIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private IIocContainer _zenIoc = null!;

    [GlobalSetup(Target = nameof(ZenIoc))]
    public void SetupZenIoc()
    {
        IIocContainer container = new IocContainer();
        container.Register<IConditionalService, ConditionalService1>("1");
        container.Register<IConditionalService, ConditionalService2>("2");
        container.Register<IConditionalService, ConditionalService3>("3");
        container.RegisterExplicit<ConditionalRoot1, ConditionalRoot1>(
            resolver => new ConditionalRoot1(resolver.Resolve<IConditionalService>("1")));
        container.RegisterExplicit<ConditionalRoot2, ConditionalRoot2>(
            resolver => new ConditionalRoot2(resolver.Resolve<IConditionalService>("2")));
        container.RegisterExplicit<ConditionalRoot3, ConditionalRoot3>(
            resolver => new ConditionalRoot3(resolver.Resolve<IConditionalService>("3")));
        container.Compile();
        _zenIoc = container;
    }

    [GlobalCleanup(Target = nameof(ZenIoc))]
    public void CleanupZenIoc() => _zenIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZenIoc)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> ZenIoc()
    {
        var first = _zenIoc.Resolve<ConditionalRoot1>();
        var second = _zenIoc.Resolve<ConditionalRoot2>();
        var third = _zenIoc.Resolve<ConditionalRoot3>();
        Validate(LibraryCatalog.ZenIoc, first, second, third);
        return new(first, second, third);
    }
}
