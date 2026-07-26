using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterGeneric(typeof(GenericService<>)).As(typeof(IGenericService<>));
        builder.RegisterGeneric(typeof(GenericRoot<>));
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Autofac()
    {
        return new(
            _autofac.Resolve<GenericRoot<int>>(),
            _autofac.Resolve<GenericRoot<float>>(),
            _autofac.Resolve<GenericRoot<object>>());
    }
}
