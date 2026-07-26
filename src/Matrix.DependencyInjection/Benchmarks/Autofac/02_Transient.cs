using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Transient1>().As<ITransient1>();
        builder.RegisterType<Transient2>().As<ITransient2>();
        builder.RegisterType<Transient3>().As<ITransient3>();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Autofac()
    {
        var first = _autofac.Resolve<ITransient1>();
        var second = _autofac.Resolve<ITransient2>();
        var third = _autofac.Resolve<ITransient3>();
        Validate(LibraryCatalog.Autofac, first);
        return new(first, second, third);
    }
}
