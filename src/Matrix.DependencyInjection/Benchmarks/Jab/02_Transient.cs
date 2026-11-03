using Jab;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private JabTransientComposition _jab = null!;

    [GlobalSetup(Target = nameof(Jab))]
    public void SetupJab() => _jab = new JabTransientComposition();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Jab)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Jab()
    {
        var first = _jab.GetService<ITransient1>();
        var second = _jab.GetService<ITransient2>();
        var third = _jab.GetService<ITransient3>();
        Validate(LibraryCatalog.Jab, first);
        return new(first, second, third);
    }
}

[ServiceProvider]
[Transient(typeof(ITransient1), typeof(Transient1))]
[Transient(typeof(ITransient2), typeof(Transient2))]
[Transient(typeof(ITransient3), typeof(Transient3))]
internal partial class JabTransientComposition;
