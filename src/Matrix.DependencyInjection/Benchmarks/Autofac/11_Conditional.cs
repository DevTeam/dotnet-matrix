using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ConditionalService1>().Keyed<IConditionalService>("1");
        builder.RegisterType<ConditionalService2>().Keyed<IConditionalService>("2");
        builder.RegisterType<ConditionalService3>().Keyed<IConditionalService>("3");
        builder.Register(context => new ConditionalRoot1(context.ResolveKeyed<IConditionalService>("1")));
        builder.Register(context => new ConditionalRoot2(context.ResolveKeyed<IConditionalService>("2")));
        builder.Register(context => new ConditionalRoot3(context.ResolveKeyed<IConditionalService>("3")));
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Autofac()
    {
        var first = _autofac.Resolve<ConditionalRoot1>();
        var second = _autofac.Resolve<ConditionalRoot2>();
        var third = _autofac.Resolve<ConditionalRoot3>();
        Validate(LibraryCatalog.Autofac, first, second, third);
        return new(first, second, third);
    }
}
