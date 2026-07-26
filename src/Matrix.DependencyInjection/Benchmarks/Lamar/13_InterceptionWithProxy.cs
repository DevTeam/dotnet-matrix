using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
            registry.For<ICalculator>().Use<Calculator>()
                .OnCreation(InterceptionProxy.Create<ICalculator>)
                .Transient());

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public ICalculator Lamar()
    {
        var calculator = _lamar.GetInstance<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Lamar, calculator, value);
        return calculator;
    }
}
