using System.Linq.Expressions;
using SimpleInjector;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Register<ICalculator, Calculator>();
        container.ExpressionBuilt += (_, args) =>
        {
            if (args.RegisteredServiceType == typeof(ICalculator))
            {
                args.Expression = Expression.Call(
                    typeof(InterceptionProxy),
                    nameof(InterceptionProxy.Create),
                    [typeof(ICalculator)],
                    args.Expression);
            }
        };
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public ICalculator SimpleInjector()
    {
        var calculator = _simpleInjector.GetInstance<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.SimpleInjector, calculator, value);
        return calculator;
    }
}
