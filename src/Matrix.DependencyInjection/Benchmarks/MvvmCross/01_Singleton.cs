using MvvmCross.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private MvxIoCContainer _mvvmCross = null!;

    [GlobalSetup(Target = nameof(MvvmCross))]
    public void SetupMvvmCross()
    {
        var container = new MvxIoCContainer(new MvxIocOptions());
        container.LazyConstructAndRegisterSingleton<ISingleton1, Singleton1>();
        container.LazyConstructAndRegisterSingleton<ISingleton2, Singleton2>();
        container.LazyConstructAndRegisterSingleton<ISingleton3, Singleton3>();
        _mvvmCross = container;
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MvvmCross)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> MvvmCross()
    {
        var first = _mvvmCross.Resolve<ISingleton1>()!;
        var second = _mvvmCross.Resolve<ISingleton2>()!;
        var third = _mvvmCross.Resolve<ISingleton3>()!;
        Validate(LibraryCatalog.MvvmCross, first, second, third);
        return new(first, second, third);
    }
}
