using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Scoped
{
    private ServiceProvider _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftDI))]
    public void SetupMicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddScoped<IScopedDependency, ScopedDependency>();
        services.AddTransient<ScopedRoot>();
        _microsoft = services.BuildServiceProvider();
    }

    [GlobalCleanup(Target = nameof(MicrosoftDI))]
    public void CleanupMicrosoftDI() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    public BenchmarkRoots<ScopedRoot, ScopedRoot> MicrosoftDI()
    {
        using var scope = _microsoft.CreateScope();
        var first = scope.ServiceProvider.GetRequiredService<ScopedRoot>();
        var second = scope.ServiceProvider.GetRequiredService<ScopedRoot>();
        Validate(LibraryCatalog.MicrosoftDi, first, second);
        return new(first, second);
    }
}
