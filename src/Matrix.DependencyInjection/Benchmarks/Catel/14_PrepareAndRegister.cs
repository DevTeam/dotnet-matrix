using Catel.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantArgumentDefaultValue

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Catel)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Catel()
    {
        using var locator = new ServiceLocator();
        locator.RegisterType<ISingleton1, Singleton1>(RegistrationType.Singleton);
        locator.RegisterType<ISingleton2, Singleton2>(RegistrationType.Singleton);
        locator.RegisterType<ISingleton3, Singleton3>(RegistrationType.Singleton);
        locator.RegisterType<ITransient1, Transient1>(RegistrationType.Transient);
        locator.RegisterType<ITransient2, Transient2>(RegistrationType.Transient);
        locator.RegisterType<ITransient3, Transient3>(RegistrationType.Transient);
        locator.RegisterType<IFirstService, FirstService>(RegistrationType.Singleton);
        locator.RegisterType<ISecondService, SecondService>(RegistrationType.Singleton);
        locator.RegisterType<IThirdService, ThirdService>(RegistrationType.Singleton);
        locator.RegisterType<SubObject1>(RegistrationType.Transient);
        locator.RegisterType<SubObject2>(RegistrationType.Transient);
        locator.RegisterType<SubObject3>(RegistrationType.Transient);
        locator.RegisterType<ComplexRoot1>(RegistrationType.Transient);
    }
}
