using MvvmCross.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private MvxIoCContainer _mvvmCross = null!;

    [GlobalSetup(Target = nameof(MvvmCross))]
    public void SetupMvvmCross()
    {
        var container = new MvxIoCContainer(new MvxIocOptions());
        container.LazyConstructAndRegisterSingleton<IFirstService, FirstService>();
        container.LazyConstructAndRegisterSingleton<ISecondService, SecondService>();
        container.LazyConstructAndRegisterSingleton<IThirdService, ThirdService>();
        container.RegisterType<SubObject1, SubObject1>();
        container.RegisterType<SubObject2, SubObject2>();
        container.RegisterType<SubObject3, SubObject3>();
        container.RegisterType<ComplexRoot1, ComplexRoot1>();
        container.RegisterType<ComplexRoot2, ComplexRoot2>();
        container.RegisterType<ComplexRoot3, ComplexRoot3>();
        _mvvmCross = container;
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MvvmCross)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> MvvmCross() =>
        new(
            _mvvmCross.Resolve<ComplexRoot1>()!,
            _mvvmCross.Resolve<ComplexRoot2>()!,
            _mvvmCross.Resolve<ComplexRoot3>()!);
}
