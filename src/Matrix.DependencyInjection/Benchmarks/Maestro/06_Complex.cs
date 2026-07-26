using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add<IFirstService>().Type<FirstService>().Singleton();
            builder.Add<ISecondService>().Type<SecondService>().Singleton();
            builder.Add<IThirdService>().Type<ThirdService>().Singleton();
            builder.Add<SubObject1>().Self().Transient();
            builder.Add<SubObject2>().Self().Transient();
            builder.Add<SubObject3>().Self().Transient();
            builder.Add<ComplexRoot1>().Self().Transient();
            builder.Add<ComplexRoot2>().Self().Transient();
            builder.Add<ComplexRoot3>().Self().Transient();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Maestro() =>
        new(
            _maestro.GetService<ComplexRoot1>(),
            _maestro.GetService<ComplexRoot2>(),
            _maestro.GetService<ComplexRoot3>());
}
