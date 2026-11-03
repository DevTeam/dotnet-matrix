using Jab;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private JabCombinedComposition _jab = null!;

    [GlobalSetup(Target = nameof(Jab))]
    public void SetupJab() => _jab = new JabCombinedComposition();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Jab)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Jab()
    {
        var first = _jab.GetService<CombinedRoot1>();
        var second = _jab.GetService<CombinedRoot2>();
        var third = _jab.GetService<CombinedRoot3>();
        Validate(LibraryCatalog.Jab, first, second, third);
        return new(first, second, third);
    }
}

[ServiceProvider]
[Singleton(typeof(ICombinedSingleton), typeof(CombinedSingleton))]
[Transient(typeof(ICombinedTransient), typeof(CombinedTransient))]
[Transient(typeof(CombinedRoot1))]
[Transient(typeof(CombinedRoot2))]
[Transient(typeof(CombinedRoot3))]
internal partial class JabCombinedComposition;
