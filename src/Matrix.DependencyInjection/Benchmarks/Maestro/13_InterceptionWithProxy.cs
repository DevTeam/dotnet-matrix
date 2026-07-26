using Castle.DynamicProxy;
using Maestro;
using Maestro.Configuration;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class InterceptionWithProxy
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
            builder.Add<ICalculator>().Type<Calculator>()
                .Proxy((calculator, generator) =>
                    generator.CreateInterfaceProxyWithTarget<ICalculator>(
                        calculator,
                        new MaestroInterceptor()))
                .Transient());

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public ICalculator Maestro()
    {
        var calculator = _maestro.GetService<ICalculator>();
        var value = calculator.Add(5, 10);
        Validate(LibraryCatalog.Maestro, calculator, value);
        return calculator;
    }

    private sealed class MaestroInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation) => invocation.Proceed();
    }
}
