using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.For<IFirstService>().Use<FirstService>().Singleton();
            registry.For<ISecondService>().Use<SecondService>().Singleton();
            registry.For<IThirdService>().Use<ThirdService>().Singleton();
            registry.For<SubObject1>().Use<SubObject1>().Transient();
            registry.For<SubObject2>().Use<SubObject2>().Transient();
            registry.For<SubObject3>().Use<SubObject3>().Transient();
            registry.For<ComplexRoot1>().Use<ComplexRoot1>().Transient();
            registry.For<ComplexRoot2>().Use<ComplexRoot2>().Transient();
            registry.For<ComplexRoot3>().Use<ComplexRoot3>().Transient();
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Lamar() =>
        new(
            _lamar.GetInstance<ComplexRoot1>(),
            _lamar.GetInstance<ComplexRoot2>(),
            _lamar.GetInstance<ComplexRoot3>());
}
