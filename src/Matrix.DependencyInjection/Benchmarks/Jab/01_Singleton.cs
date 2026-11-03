using Jab;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private JabSingletonComposition _jab = null!;

    [GlobalSetup(Target = nameof(Jab))]
    public void SetupJab() => _jab = new JabSingletonComposition();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Jab)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Jab()
    {
        var first = _jab.GetService<ISingleton1>();
        var second = _jab.GetService<ISingleton2>();
        var third = _jab.GetService<ISingleton3>();
        Validate(LibraryCatalog.Jab, first, second, third);
        return new(first, second, third);
    }
}

[ServiceProvider]
[Singleton(typeof(ISingleton1), typeof(Singleton1))]
[Singleton(typeof(ISingleton2), typeof(Singleton2))]
[Singleton(typeof(ISingleton3), typeof(Singleton3))]
internal partial class JabSingletonComposition;
