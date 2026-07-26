using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private ServiceProvider _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftDI))]
    public void SetupMicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IFirstService, FirstService>();
        services.AddSingleton<ISecondService, SecondService>();
        services.AddSingleton<IThirdService, ThirdService>();
        services.AddTransient<SubObject1>();
        services.AddTransient<SubObject2>();
        services.AddTransient<SubObject3>();
        services.AddTransient<ComplexRoot1>();
        services.AddTransient<ComplexRoot2>();
        services.AddTransient<ComplexRoot3>();
        _microsoft = services.BuildServiceProvider();
    }

    [GlobalCleanup(Target = nameof(MicrosoftDI))]
    public void CleanupMicrosoftDI() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> MicrosoftDI()
    {
        return new(
            _microsoft.GetRequiredService<ComplexRoot1>(),
            _microsoft.GetRequiredService<ComplexRoot2>(),
            _microsoft.GetRequiredService<ComplexRoot3>());
    }
}
