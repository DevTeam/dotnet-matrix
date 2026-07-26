using Stashbox;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private StashboxContainer _stashbox = null!;

    [GlobalSetup(Target = nameof(Stashbox))]
    public void SetupStashbox()
    {
        var container = new StashboxContainer();
        container.Register<ICalculator, Calculator>();
        container.RegisterDecorator(
            typeof(ICalculator),
            InterceptionProxy.TypeOf<ICalculator>(),
            configurator => configurator.WithFactory<ICalculator>(InterceptionProxy.Create));
        _stashbox = container;
    }

    [GlobalCleanup(Target = nameof(Stashbox))]
    public void CleanupStashbox() => _stashbox.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Stashbox)]
    public ICalculator Stashbox()
    {
        var calculator = _stashbox.Resolve<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Stashbox, calculator, value);
        return calculator;
    }
}
