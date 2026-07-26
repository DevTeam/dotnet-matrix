using MvvmCross.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private MvxIoCContainer _mvvmCross = null!;

    [GlobalSetup(Target = nameof(MvvmCross))]
    public void SetupMvvmCross()
    {
        var container = new MvxIoCContainer(new MvxIocOptions());
        container.RegisterType<ITransient1, Transient1>();
        container.RegisterType<ITransient2, Transient2>();
        container.RegisterType<ITransient3, Transient3>();
        _mvvmCross = container;
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MvvmCross)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> MvvmCross()
    {
        var first = _mvvmCross.Resolve<ITransient1>()!;
        var second = _mvvmCross.Resolve<ITransient2>()!;
        var third = _mvvmCross.Resolve<ITransient3>()!;
        Validate(LibraryCatalog.MvvmCross, first);
        return new(first, second, third);
    }
}
