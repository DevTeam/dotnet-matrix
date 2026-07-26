using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private ServiceProvider _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftDI))]
    public void SetupMicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddKeyedTransient<IConditionalService, ConditionalService1>("1");
        services.AddKeyedTransient<IConditionalService, ConditionalService2>("2");
        services.AddKeyedTransient<IConditionalService, ConditionalService3>("3");
        services.AddTransient(provider =>
            new ConditionalRoot1(provider.GetRequiredKeyedService<IConditionalService>("1")));
        services.AddTransient(provider =>
            new ConditionalRoot2(provider.GetRequiredKeyedService<IConditionalService>("2")));
        services.AddTransient(provider =>
            new ConditionalRoot3(provider.GetRequiredKeyedService<IConditionalService>("3")));
        _microsoft = services.BuildServiceProvider();
    }

    [GlobalCleanup(Target = nameof(MicrosoftDI))]
    public void CleanupMicrosoftDI() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> MicrosoftDI()
    {
        var first = _microsoft.GetRequiredService<ConditionalRoot1>();
        var second = _microsoft.GetRequiredService<ConditionalRoot2>();
        var third = _microsoft.GetRequiredService<ConditionalRoot3>();
        Validate(LibraryCatalog.MicrosoftDi, first, second, third);
        return new(first, second, third);
    }
}
