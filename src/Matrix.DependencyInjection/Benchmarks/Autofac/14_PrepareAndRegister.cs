using Autofac;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Autofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Singleton1>().As<ISingleton1>().SingleInstance();
        builder.RegisterType<Singleton2>().As<ISingleton2>().SingleInstance();
        builder.RegisterType<Singleton3>().As<ISingleton3>().SingleInstance();
        builder.RegisterType<Transient1>().As<ITransient1>();
        builder.RegisterType<Transient2>().As<ITransient2>();
        builder.RegisterType<Transient3>().As<ITransient3>();
        builder.RegisterType<FirstService>().As<IFirstService>().SingleInstance();
        builder.RegisterType<SecondService>().As<ISecondService>().SingleInstance();
        builder.RegisterType<ThirdService>().As<IThirdService>().SingleInstance();
        builder.RegisterType<SubObject1>();
        builder.RegisterType<SubObject2>();
        builder.RegisterType<SubObject3>();
        builder.RegisterType<ComplexRoot1>();
        using var container = builder.Build();
    }
}
