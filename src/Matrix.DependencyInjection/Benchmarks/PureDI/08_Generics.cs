// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Transient<GenericService<TT>>()
            .Root<GenericRoot<int>>(nameof(PureInt))
            .Root<GenericRoot<float>>(nameof(PureFloat))
            .Root<GenericRoot<object>>(nameof(PureObject));

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> PureDI()
    {
        return new(PureInt, PureFloat, PureObject);
    }
}
