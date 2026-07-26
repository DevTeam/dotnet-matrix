using DryIoc;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private Container _dryIoc = null!;

    [GlobalSetup(Target = nameof(DryIoc))]
    public void SetupDryIoc()
    {
        var container = new Container();
        container.Register<ICalculator, Calculator>(Reuse.Transient);
        container.Register(
            Made.Of(() => InterceptionProxy.Create(Arg.Of<ICalculator>())),
            setup: Setup.Decorator);
        _dryIoc = container;
    }

    [GlobalCleanup(Target = nameof(DryIoc))]
    public void CleanupDryIoc() => _dryIoc.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DryIoc)]
    public ICalculator DryIoc()
    {
        var calculator = _dryIoc.Resolve<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.DryIoc, calculator, value);
        return calculator;
    }
}
