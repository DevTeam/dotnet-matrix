using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add<ISingleton1>().Type<Singleton1>().Singleton();
            builder.Add<ISingleton2>().Type<Singleton2>().Singleton();
            builder.Add<ISingleton3>().Type<Singleton3>().Singleton();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Maestro()
    {
        var first = _maestro.GetService<ISingleton1>();
        var second = _maestro.GetService<ISingleton2>();
        var third = _maestro.GetService<ISingleton3>();
        Validate(LibraryCatalog.Maestro, first, second, third);
        return new(first, second, third);
    }
}
