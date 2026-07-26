using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private IContainer _autofac = null!;

    [GlobalSetup(Target = nameof(Autofac))]
    public void SetupAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<PassThroughInterceptor>();
        builder.RegisterType<Calculator>()
            .As<ICalculator>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(PassThroughInterceptor));
        _autofac = builder.Build();
    }

    [GlobalCleanup(Target = nameof(Autofac))]
    public void CleanupAutofac() => _autofac.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Autofac)]
    public ICalculator Autofac()
    {
        var calculator = _autofac.Resolve<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Autofac, calculator, value);
        return calculator;
    }

    private sealed class PassThroughInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation) => invocation.Proceed();
    }
}
