using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Plugin1>().As<IPlugin>();
        builder.RegisterType<Plugin2>().As<IPlugin>();
        builder.RegisterType<Plugin3>().As<IPlugin>();
        builder.RegisterType<Plugin4>().As<IPlugin>();
        builder.RegisterType<Plugin5>().As<IPlugin>();
        builder.RegisterType<ArrayRoot1>();
        builder.RegisterType<ArrayRoot2>();
        builder.RegisterType<ArrayRoot3>();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Autofac()
    {
        var first = _autofac.Resolve<ArrayRoot1>();
        var second = _autofac.Resolve<ArrayRoot2>();
        var third = _autofac.Resolve<ArrayRoot3>();
        Validate(LibraryCatalog.Autofac, first, second, third);
        return new(first, second, third);
    }
}
