using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 Maestro()
    {
        using var container = new Container(builder =>
        {
            builder.Add<ISingleton1>().Type<Singleton1>().Singleton();
            builder.Add<ISingleton2>().Type<Singleton2>().Singleton();
            builder.Add<ISingleton3>().Type<Singleton3>().Singleton();
            builder.Add<ITransient1>().Type<Transient1>().Transient();
            builder.Add<ITransient2>().Type<Transient2>().Transient();
            builder.Add<ITransient3>().Type<Transient3>().Transient();
            builder.Add<IFirstService>().Type<FirstService>().Singleton();
            builder.Add<ISecondService>().Type<SecondService>().Singleton();
            builder.Add<IThirdService>().Type<ThirdService>().Singleton();
            builder.Add<SubObject1>().Self().Transient();
            builder.Add<SubObject2>().Self().Transient();
            builder.Add<SubObject3>().Self().Transient();
            builder.Add<ComplexRoot1>().Self().Transient();
        });
        return container.GetService<ISingleton1>();
    }
}
