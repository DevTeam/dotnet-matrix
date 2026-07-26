using AopAlliance.Intercept;
using Spring.Aop.Framework;
using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    [SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments")]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("target", new RootObjectDefinition(typeof(Calculator), false));
        factory.RegisterObjectDefinition(
            "interceptor",
            new RootObjectDefinition(typeof(SpringInterceptor), true));
        var proxy = new RootObjectDefinition(typeof(ProxyFactoryObject), true);
        proxy.PropertyValues.Add("TargetName", "target");
        proxy.PropertyValues.Add("InterceptorNames", new[] { "interceptor" });
        proxy.PropertyValues.Add("ProxyInterfaces", new[] { typeof(ICalculator).AssemblyQualifiedName });
        factory.RegisterObjectDefinition("calculator", proxy);
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public ICalculator Spring()
    {
        var calculator = (ICalculator)_spring.GetObject("calculator");
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Spring, calculator, value);
        return calculator;
    }

    private sealed class SpringInterceptor : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation) => invocation.Proceed();
    }
}
