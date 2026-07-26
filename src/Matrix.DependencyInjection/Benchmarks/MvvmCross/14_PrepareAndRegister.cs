using MvvmCross.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MvvmCross)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void MvvmCross()
    {
        var container = new MvxIoCContainer(new MvxIocOptions());
        container.LazyConstructAndRegisterSingleton<ISingleton1, Singleton1>();
        container.LazyConstructAndRegisterSingleton<ISingleton2, Singleton2>();
        container.LazyConstructAndRegisterSingleton<ISingleton3, Singleton3>();
        container.RegisterType<ITransient1, Transient1>();
        container.RegisterType<ITransient2, Transient2>();
        container.RegisterType<ITransient3, Transient3>();
        container.LazyConstructAndRegisterSingleton<IFirstService, FirstService>();
        container.LazyConstructAndRegisterSingleton<ISecondService, SecondService>();
        container.LazyConstructAndRegisterSingleton<IThirdService, ThirdService>();
        container.RegisterType<SubObject1, SubObject1>();
        container.RegisterType<SubObject2, SubObject2>();
        container.RegisterType<SubObject3, SubObject3>();
        container.RegisterType<ComplexRoot1, ComplexRoot1>();
    }
}
