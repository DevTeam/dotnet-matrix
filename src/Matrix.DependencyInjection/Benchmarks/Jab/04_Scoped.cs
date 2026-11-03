using Jab;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private JabScopedComposition _jab = null!;

    [GlobalSetup(Target = nameof(Jab))]
    public void SetupJab() => _jab = new JabScopedComposition();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Jab)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> Jab()
    {
        using var scope = _jab.CreateScope();
        var first = scope.GetService<ScopedRoot>();
        var second = scope.GetService<ScopedRoot>();
        Validate(LibraryCatalog.Jab, first, second);
        return new(first, second);
    }
}

[ServiceProvider]
[Scoped(typeof(IScopedDependency), typeof(ScopedDependency))]
[Transient(typeof(ScopedRoot))]
internal partial class JabScopedComposition;
