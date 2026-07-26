using Singularity;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Singularity)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 Singularity()
    {
        using var container = new Container(builder =>
        {
            builder.Register<ISingleton1, Singleton1>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ISingleton2, Singleton2>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ISingleton3, Singleton3>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ITransient1, Transient1>(c => c.With(Lifetimes.Transient));
            builder.Register<ITransient2, Transient2>(c => c.With(Lifetimes.Transient));
            builder.Register<ITransient3, Transient3>(c => c.With(Lifetimes.Transient));
            builder.Register<IFirstService, FirstService>(c => c.With(Lifetimes.PerContainer));
            builder.Register<ISecondService, SecondService>(c => c.With(Lifetimes.PerContainer));
            builder.Register<IThirdService, ThirdService>(c => c.With(Lifetimes.PerContainer));
            builder.Register<SubObject1>(c => c.With(Lifetimes.Transient));
            builder.Register<SubObject2>(c => c.With(Lifetimes.Transient));
            builder.Register<SubObject3>(c => c.With(Lifetimes.Transient));
            builder.Register<ComplexRoot1>(c => c.With(Lifetimes.Transient));
        });
        return container.GetInstance<ISingleton1>()!;
    }
}
