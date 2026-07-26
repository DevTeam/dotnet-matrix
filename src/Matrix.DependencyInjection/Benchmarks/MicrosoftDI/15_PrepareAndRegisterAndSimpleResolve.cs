using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftDi)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 MicrosoftDI()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton1, Singleton1>();
        services.AddSingleton<ISingleton2, Singleton2>();
        services.AddSingleton<ISingleton3, Singleton3>();
        services.AddTransient<ITransient1, Transient1>();
        services.AddTransient<ITransient2, Transient2>();
        services.AddTransient<ITransient3, Transient3>();
        services.AddSingleton<IFirstService, FirstService>();
        services.AddSingleton<ISecondService, SecondService>();
        services.AddSingleton<IThirdService, ThirdService>();
        services.AddTransient<SubObject1>();
        services.AddTransient<SubObject2>();
        services.AddTransient<SubObject3>();
        services.AddTransient<ComplexRoot1>();
        using var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ISingleton1>();
    }
}
