// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameterInPartialMethod
namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private readonly PureDiInterceptionComposition _pure = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PureDi)]
    public ICalculator PureDI()
    {
        var calculator = _pure.Root;
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.PureDi, calculator, value);
        return calculator;
    }
}

internal partial class PureDiInterceptionComposition
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Transient(ICalculator (Calculator calculator) => InterceptionProxy.Create<ICalculator>(calculator))
            .Root<ICalculator>(nameof(Root));
}
