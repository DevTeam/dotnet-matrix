// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ICalculator HandCoded()
    {
        var calculator = InterceptionProxy.Create<ICalculator>(new Calculator());
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.HandCoded, calculator, value);
        return calculator;
    }
}
