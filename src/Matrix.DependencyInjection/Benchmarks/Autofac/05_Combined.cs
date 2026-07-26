using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<CombinedSingleton>().As<ICombinedSingleton>().SingleInstance();
        builder.RegisterType<CombinedTransient>().As<ICombinedTransient>();
        builder.RegisterType<CombinedRoot1>();
        builder.RegisterType<CombinedRoot2>();
        builder.RegisterType<CombinedRoot3>();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Autofac()
    {
        var first = _autofac.Resolve<CombinedRoot1>();
        var second = _autofac.Resolve<CombinedRoot2>();
        var third = _autofac.Resolve<CombinedRoot3>();
        Validate(LibraryCatalog.Autofac, first, second, third);
        return new(first, second, third);
    }
}
