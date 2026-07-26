using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Infrastructure.Language;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind<ICalculator>().To<Calculator>()
            .InTransientScope()
            .Intercept()
            .With(new NinjectInterceptor());
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public ICalculator Ninject()
    {
        var calculator = _ninject.Get<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Ninject, calculator, value);
        return calculator;
    }

    private sealed class NinjectInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation) => invocation.Proceed();
    }
}
