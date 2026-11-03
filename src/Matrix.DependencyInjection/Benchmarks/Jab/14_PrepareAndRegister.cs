using Jab;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegister
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Jab)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Jab()
    {
        var composition = new JabPrepareComposition();
    }
}

[ServiceProvider]
[Singleton(typeof(ISingleton1), typeof(Singleton1))]
[Singleton(typeof(ISingleton2), typeof(Singleton2))]
[Singleton(typeof(ISingleton3), typeof(Singleton3))]
[Transient(typeof(ITransient1), typeof(Transient1))]
[Transient(typeof(ITransient2), typeof(Transient2))]
[Transient(typeof(ITransient3), typeof(Transient3))]
[Singleton(typeof(IFirstService), typeof(FirstService))]
[Singleton(typeof(ISecondService), typeof(SecondService))]
[Singleton(typeof(IThirdService), typeof(ThirdService))]
[Transient(typeof(SubObject1))]
[Transient(typeof(SubObject2))]
[Transient(typeof(SubObject3))]
[Transient(typeof(ComplexRoot1))]
internal partial class JabPrepareComposition;
