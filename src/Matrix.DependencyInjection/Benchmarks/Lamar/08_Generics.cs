using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For(typeof(IGenericService<>)).Use(typeof(GenericService<>));
            registry.For(typeof(GenericRoot<>)).Use(typeof(GenericRoot<>));
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Lamar() =>
        new(
            _lamar.GetInstance<GenericRoot<int>>(),
            _lamar.GetInstance<GenericRoot<float>>(),
            _lamar.GetInstance<GenericRoot<object>>());
}
