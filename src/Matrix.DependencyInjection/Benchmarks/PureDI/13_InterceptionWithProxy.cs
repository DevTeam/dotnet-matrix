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
        // OnDependencyInjection = On
        // OnDependencyInjectionContractTypeNameWildcard = *ICalculator
        DI.Setup()
            .Bind<ICalculator>().To<Calculator>()
            .Root<ICalculator>(nameof(Root));

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private partial T OnDependencyInjection<T>(
        in T value,
        object? tag,
        Lifetime lifetime) =>
        InterceptionProxy.Create(value);
}
