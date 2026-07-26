using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Singleton1>().As<ISingleton1>().SingleInstance();
        builder.RegisterType<Singleton2>().As<ISingleton2>().SingleInstance();
        builder.RegisterType<Singleton3>().As<ISingleton3>().SingleInstance();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Autofac()
    {
        var first = _autofac.Resolve<ISingleton1>();
        var second = _autofac.Resolve<ISingleton2>();
        var third = _autofac.Resolve<ISingleton3>();
        Validate(LibraryCatalog.Autofac, first, second, third);
        return new(first, second, third);
    }
}
