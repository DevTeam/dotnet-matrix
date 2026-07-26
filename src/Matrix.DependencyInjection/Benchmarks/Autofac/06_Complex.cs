using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<FirstService>().As<IFirstService>().SingleInstance();
        builder.RegisterType<SecondService>().As<ISecondService>().SingleInstance();
        builder.RegisterType<ThirdService>().As<IThirdService>().SingleInstance();
        builder.RegisterType<SubObject1>();
        builder.RegisterType<SubObject2>();
        builder.RegisterType<SubObject3>();
        builder.RegisterType<ComplexRoot1>();
        builder.RegisterType<ComplexRoot2>();
        builder.RegisterType<ComplexRoot3>();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Autofac()
    {
        return new(
            _autofac.Resolve<ComplexRoot1>(),
            _autofac.Resolve<ComplexRoot2>(),
            _autofac.Resolve<ComplexRoot3>());
    }
}
