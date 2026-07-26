using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Windsor()
    {
        using var container = new WindsorContainer();
        container.Register(
            Component.For<ISingleton1>().ImplementedBy<Singleton1>().LifestyleSingleton(),
            Component.For<ISingleton2>().ImplementedBy<Singleton2>().LifestyleSingleton(),
            Component.For<ISingleton3>().ImplementedBy<Singleton3>().LifestyleSingleton(),
            Component.For<ITransient1>().ImplementedBy<Transient1>().LifestyleTransient(),
            Component.For<ITransient2>().ImplementedBy<Transient2>().LifestyleTransient(),
            Component.For<ITransient3>().ImplementedBy<Transient3>().LifestyleTransient(),
            Component.For<IFirstService>().ImplementedBy<FirstService>().LifestyleSingleton(),
            Component.For<ISecondService>().ImplementedBy<SecondService>().LifestyleSingleton(),
            Component.For<IThirdService>().ImplementedBy<ThirdService>().LifestyleSingleton(),
            Component.For<SubObject1>().LifestyleTransient(),
            Component.For<SubObject2>().LifestyleTransient(),
            Component.For<SubObject3>().LifestyleTransient(),
            Component.For<ComplexRoot1>().LifestyleTransient());
    }
}
