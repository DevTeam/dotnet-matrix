using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private ServiceProvider _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftDI))]
    public void SetupMicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransient1, Transient1>();
        services.AddTransient<ITransient2, Transient2>();
        services.AddTransient<ITransient3, Transient3>();
        _microsoft = services.BuildServiceProvider();
    }

    [GlobalCleanup(Target = nameof(MicrosoftDI))]
    public void CleanupMicrosoftDI() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> MicrosoftDI()
    {
        var first = _microsoft.GetRequiredService<ITransient1>();
        var second = _microsoft.GetRequiredService<ITransient2>();
        var third = _microsoft.GetRequiredService<ITransient3>();
        Validate(LibraryCatalog.MicrosoftDi, first);
        return new(first, second, third);
    }
}
