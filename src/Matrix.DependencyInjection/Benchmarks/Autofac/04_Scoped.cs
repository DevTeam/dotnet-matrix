using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ScopedDependency>().As<IScopedDependency>().InstancePerLifetimeScope();
        builder.RegisterType<ScopedRoot>();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Autofac()
    {
        using var scope = _autofac.BeginLifetimeScope();
        var first = scope.Resolve<ScopedRoot>();
        var second = scope.Resolve<ScopedRoot>();
        Validate(LibraryCatalog.Autofac, first, second);
        return new(first, second);
    }
}
