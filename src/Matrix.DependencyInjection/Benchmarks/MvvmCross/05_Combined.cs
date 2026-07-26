using MvvmCross.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private MvxIoCContainer _mvvmCross = null!;

    [GlobalSetup(Target = nameof(MvvmCross))]
    public void SetupMvvmCross()
    {
        var container = new MvxIoCContainer(new MvxIocOptions());
        container.LazyConstructAndRegisterSingleton<ICombinedSingleton, CombinedSingleton>();
        container.RegisterType<ICombinedTransient, CombinedTransient>();
        container.RegisterType<CombinedRoot1, CombinedRoot1>();
        container.RegisterType<CombinedRoot2, CombinedRoot2>();
        container.RegisterType<CombinedRoot3, CombinedRoot3>();
        _mvvmCross = container;
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MvvmCross)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> MvvmCross()
    {
        var first = _mvvmCross.Resolve<CombinedRoot1>()!;
        var second = _mvvmCross.Resolve<CombinedRoot2>()!;
        var third = _mvvmCross.Resolve<CombinedRoot3>()!;
        Validate(LibraryCatalog.MvvmCross, first, second, third);
        return new(first, second, third);
    }
}
