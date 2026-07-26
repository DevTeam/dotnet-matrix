using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add<ITransient1>().Type<Transient1>().Transient();
            builder.Add<ITransient2>().Type<Transient2>().Transient();
            builder.Add<ITransient3>().Type<Transient3>().Transient();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Maestro()
    {
        var first = _maestro.GetService<ITransient1>();
        var second = _maestro.GetService<ITransient2>();
        var third = _maestro.GetService<ITransient3>();
        Validate(LibraryCatalog.Maestro, first);
        return new(first, second, third);
    }
}
