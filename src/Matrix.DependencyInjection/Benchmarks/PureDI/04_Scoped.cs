// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private readonly ScopedPureComposition _pure = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> PureDI()
    {
        using var scope = new PureDiScopedScope(_pure);
        var first = scope.PureRoot;
        var second = scope.PureRoot;
        Validate(LibraryCatalog.PureDi, first, second);
        return new(first, second);
    }
}

internal partial class ScopedPureComposition
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Scoped<ScopedDependency>()
            .Root<ScopedRoot>(nameof(PureRoot));
}

internal sealed class PureDiScopedScope(ScopedPureComposition parent)
    : ScopedPureComposition(parent);
