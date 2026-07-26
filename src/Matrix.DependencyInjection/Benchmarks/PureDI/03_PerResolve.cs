// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class PerResolve
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Bind<IPerResolveDependency>().As(Lifetime.PerResolve).To<PerResolveDependency>()
            .Root<PerResolveRoot>(nameof(PureRoot));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public PerResolveRoot PureDI()
    {
        var root = PureRoot;
        Validate(LibraryCatalog.PureDi, root);
        return root;
    }
}
