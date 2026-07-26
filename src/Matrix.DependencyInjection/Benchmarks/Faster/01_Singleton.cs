using Faster.Ioc;
using FasterLifetime = Faster.Ioc.Contracts.Lifetime;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private Container _faster = null!;

    [GlobalSetup(Target = nameof(Faster))]
    public void SetupFaster()
    {
        var container = new Container();
        container.Register<ISingleton1, Singleton1>(FasterLifetime.Singleton);
        container.Register<ISingleton2, Singleton2>(FasterLifetime.Singleton);
        container.Register<ISingleton3, Singleton3>(FasterLifetime.Singleton);
        _faster = container;
    }

    [GlobalCleanup(Target = nameof(Faster))]
    public void CleanupFaster() => _faster.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FasterIoc)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Faster()
    {
        var first = _faster.Resolve<ISingleton1>();
        var second = _faster.Resolve<ISingleton2>();
        var third = _faster.Resolve<ISingleton3>();
        Validate(LibraryCatalog.FasterIoc, first, second, third);
        return new(first, second, third);
    }
}
