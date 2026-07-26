using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private WindsorContainer _windsor = null!;

    [GlobalSetup(Target = nameof(Windsor))]
    public void SetupWindsor()
    {
        var container = new WindsorContainer();
        container.Register(
            Component.For<WindsorInterceptor>().LifestyleSingleton(),
            Component.For<ICalculator>()
                .ImplementedBy<Calculator>()
                .Interceptors<WindsorInterceptor>()
                .LifestyleTransient());
        _windsor = container;
    }

    [GlobalCleanup(Target = nameof(Windsor))]
    public void CleanupWindsor() => _windsor.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Windsor)]
    public ICalculator Windsor()
    {
        var calculator = _windsor.Resolve<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Windsor, calculator, value);
        return calculator;
    }

    private sealed class WindsorInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation) => invocation.Proceed();
    }
}
