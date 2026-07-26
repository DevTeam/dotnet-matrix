using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 Ninject()
    {
        using var kernel = new StandardKernel();
        kernel.Bind<ISingleton1>().To<Singleton1>().InSingletonScope();
        kernel.Bind<ISingleton2>().To<Singleton2>().InSingletonScope();
        kernel.Bind<ISingleton3>().To<Singleton3>().InSingletonScope();
        kernel.Bind<ITransient1>().To<Transient1>().InTransientScope();
        kernel.Bind<ITransient2>().To<Transient2>().InTransientScope();
        kernel.Bind<ITransient3>().To<Transient3>().InTransientScope();
        kernel.Bind<IFirstService>().To<FirstService>().InSingletonScope();
        kernel.Bind<ISecondService>().To<SecondService>().InSingletonScope();
        kernel.Bind<IThirdService>().To<ThirdService>().InSingletonScope();
        kernel.Bind<SubObject1>().ToSelf().InTransientScope();
        kernel.Bind<SubObject2>().ToSelf().InTransientScope();
        kernel.Bind<SubObject3>().ToSelf().InTransientScope();
        kernel.Bind<ComplexRoot1>().ToSelf().InTransientScope();
        return kernel.Get<ISingleton1>();
    }
}
