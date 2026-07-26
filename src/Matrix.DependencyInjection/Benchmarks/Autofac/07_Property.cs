using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<PropertyServiceA>();
        builder.RegisterType<PropertyServiceB>();
        builder.RegisterType<PropertyServiceC>();
        builder.RegisterType<PropertyRoot1>().PropertiesAutowired();
        builder.RegisterType<PropertyRoot2>().PropertiesAutowired();
        builder.RegisterType<PropertyRoot3>().PropertiesAutowired();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Autofac()
    {
        var first = _autofac.Resolve<PropertyRoot1>();
        var second = _autofac.Resolve<PropertyRoot2>();
        var third = _autofac.Resolve<PropertyRoot3>();
        Validate(LibraryCatalog.Autofac, first, second, third);
        return new(first, second, third);
    }
}
