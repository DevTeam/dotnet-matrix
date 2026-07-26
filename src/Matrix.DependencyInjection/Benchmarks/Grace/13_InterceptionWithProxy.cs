using Grace.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBeMadeStatic.Local

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private DependencyInjectionContainer _grace = null!;

    [GlobalSetup(Target = nameof(Grace))]
    public void SetupGrace()
    {
        var container = new DependencyInjectionContainer();
        container.Configure(block =>
        {
            block.Export<Calculator>().As<ICalculator>();
            block.ExportDecorator<ICalculator>(new GraceProxyFactory().Create);
        });
        _grace = container;
    }

    [GlobalCleanup(Target = nameof(Grace))]
    public void CleanupGrace() => _grace.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Grace)]
    public ICalculator Grace()
    {
        var calculator = _grace.Locate<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Grace, calculator, value);
        return calculator;
    }

    // Grace builds the decorator call from the delegate target, so the factory must be an instance method.
    private sealed class GraceProxyFactory
    {
        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public ICalculator Create(ICalculator calculator) => InterceptionProxy.Create(calculator);
    }
}
