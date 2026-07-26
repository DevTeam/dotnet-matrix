using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private ServiceProvider _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftDI))]
    public void SetupMicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddTransient(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddTransient(typeof(GenericRoot<>));
        _microsoft = services.BuildServiceProvider();
    }

    [GlobalCleanup(Target = nameof(MicrosoftDI))]
    public void CleanupMicrosoftDI() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> MicrosoftDI()
    {
        return new(
            _microsoft.GetRequiredService<GenericRoot<int>>(),
            _microsoft.GetRequiredService<GenericRoot<float>>(),
            _microsoft.GetRequiredService<GenericRoot<object>>());
    }
}
