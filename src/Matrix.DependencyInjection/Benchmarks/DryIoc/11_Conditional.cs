using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<IConditionalService, ConditionalService1>(
            Reuse.Transient,
            setup: Setup.With(condition: request =>
                request.Parent.ImplementationType == typeof(ConditionalRoot1)));
        container.Register<IConditionalService, ConditionalService2>(
            Reuse.Transient,
            setup: Setup.With(condition: request =>
                request.Parent.ImplementationType == typeof(ConditionalRoot2)));
        container.Register<IConditionalService, ConditionalService3>(
            Reuse.Transient,
            setup: Setup.With(condition: request =>
                request.Parent.ImplementationType == typeof(ConditionalRoot3)));
        container.Register<ConditionalRoot1>(Reuse.Transient);
        container.Register<ConditionalRoot2>(Reuse.Transient);
        container.Register<ConditionalRoot3>(Reuse.Transient);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> DryIoc()
    {
        var first = _dryIoc.Resolve<ConditionalRoot1>();
        var second = _dryIoc.Resolve<ConditionalRoot2>();
        var third = _dryIoc.Resolve<ConditionalRoot3>();
        Validate(LibraryCatalog.DryIoc, first, second, third);
        return new(first, second, third);
    }
}
