using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<ISingleton1>().ImplementedBy<Singleton1>().LifestyleSingleton(),
            Component.For<ISingleton2>().ImplementedBy<Singleton2>().LifestyleSingleton(),
            Component.For<ISingleton3>().ImplementedBy<Singleton3>().LifestyleSingleton());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Windsor()
    {
        var first = _windsor.Resolve<ISingleton1>();
        var second = _windsor.Resolve<ISingleton2>();
        var third = _windsor.Resolve<ISingleton3>();
        Validate(LibraryCatalog.Windsor, first, second, third);
        return new(first, second, third);
    }
}
