using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private ServiceProvider _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftDI))]
    public void SetupMicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ICombinedSingleton, CombinedSingleton>();
        services.AddTransient<ICombinedTransient, CombinedTransient>();
        services.AddTransient<CombinedRoot1>();
        services.AddTransient<CombinedRoot2>();
        services.AddTransient<CombinedRoot3>();
        _microsoft = services.BuildServiceProvider();
    }

    [GlobalCleanup(Target = nameof(MicrosoftDI))]
    public void CleanupMicrosoftDI() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> MicrosoftDI()
    {
        var first = _microsoft.GetRequiredService<CombinedRoot1>();
        var second = _microsoft.GetRequiredService<CombinedRoot2>();
        var third = _microsoft.GetRequiredService<CombinedRoot3>();
        Validate(LibraryCatalog.MicrosoftDi, first, second, third);
        return new(first, second, third);
    }
}
