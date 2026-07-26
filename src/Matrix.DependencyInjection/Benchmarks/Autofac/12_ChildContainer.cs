using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class ChildContainer
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ParentValue>().As<IChildValue>();
        builder.RegisterType<ChildRoot>();
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public BenchmarkRoots<ChildRoot, ChildRoot> Autofac()
    {
        var parent = _autofac.Resolve<ChildRoot>();
        using var child = _autofac.BeginLifetimeScope(
            builder => builder.RegisterType<ChildValue>().As<IChildValue>());
        var root = child.Resolve<ChildRoot>();
        Validate(LibraryCatalog.Autofac, parent, root);
        return new(parent, root);
    }
}
