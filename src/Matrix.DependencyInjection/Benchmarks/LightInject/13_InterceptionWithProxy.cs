using LightInject;
using LightInject.Interception;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private ServiceContainer _lightInject = null!;

    [GlobalSetup(Target = nameof(LightInject))]
    public void SetupLightInject()
    {
        var container = new ServiceContainer();
        container.Register<ICalculator, Calculator>();
        container.Intercept(
            registration => registration.ServiceType == typeof(ICalculator),
            _ => new LightInjectInterceptor());
        _lightInject = container;
    }

    [GlobalCleanup(Target = nameof(LightInject))]
    public void CleanupLightInject() => _lightInject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LightInject)]
    public ICalculator LightInject()
    {
        var calculator = _lightInject.GetInstance<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.LightInject, calculator, value);
        return calculator;
    }

    private sealed class LightInjectInterceptor : IInterceptor
    {
        public object Invoke(IInvocationInfo invocationInfo) => invocationInfo.Proceed();
    }
}
