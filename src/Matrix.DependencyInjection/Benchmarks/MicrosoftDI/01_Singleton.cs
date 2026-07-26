using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private ServiceProvider _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftDI))]
    public void SetupMicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton1, Singleton1>();
        services.AddSingleton<ISingleton2, Singleton2>();
        services.AddSingleton<ISingleton3, Singleton3>();
        _microsoft = services.BuildServiceProvider();
    }

    [GlobalCleanup(Target = nameof(MicrosoftDI))]
    public void CleanupMicrosoftDI() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> MicrosoftDI()
    {
        var first = _microsoft.GetRequiredService<ISingleton1>();
        var second = _microsoft.GetRequiredService<ISingleton2>();
        var third = _microsoft.GetRequiredService<ISingleton3>();
        Validate(LibraryCatalog.MicrosoftDi, first, second, third);
        return new(first, second, third);
    }
}
