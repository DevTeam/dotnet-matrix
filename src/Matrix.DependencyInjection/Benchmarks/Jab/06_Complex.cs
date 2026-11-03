using Jab;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private JabComplexComposition _jab = null!;

    [GlobalSetup(Target = nameof(Jab))]
    public void SetupJab() => _jab = new JabComplexComposition();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Jab)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Jab()
        => new(
            _jab.GetService<ComplexRoot1>(),
            _jab.GetService<ComplexRoot2>(),
            _jab.GetService<ComplexRoot3>());
}

[ServiceProvider]
[Singleton(typeof(IFirstService), typeof(FirstService))]
[Singleton(typeof(ISecondService), typeof(SecondService))]
[Singleton(typeof(IThirdService), typeof(ThirdService))]
[Transient(typeof(SubObject1))]
[Transient(typeof(SubObject2))]
[Transient(typeof(SubObject3))]
[Transient(typeof(ComplexRoot1))]
[Transient(typeof(ComplexRoot2))]
[Transient(typeof(ComplexRoot3))]
internal partial class JabComplexComposition;
