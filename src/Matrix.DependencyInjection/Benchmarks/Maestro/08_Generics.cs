using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add(typeof(IGenericService<>)).Type(typeof(GenericService<>)).Transient();
            builder.Add(typeof(GenericRoot<>)).Self().Transient();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Maestro() =>
        new(
            _maestro.GetService<GenericRoot<int>>(),
            _maestro.GetService<GenericRoot<float>>(),
            _maestro.GetService<GenericRoot<object>>());
}
