using Catel.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable RedundantArgumentDefaultValue

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private ServiceLocator _catel = null!;

    [GlobalSetup(Target = nameof(Catel))]
    public void SetupCatel()
    {
        var locator = new ServiceLocator();
        locator.RegisterType<IFirstService, FirstService>(RegistrationType.Singleton);
        locator.RegisterType<ISecondService, SecondService>(RegistrationType.Singleton);
        locator.RegisterType<IThirdService, ThirdService>(RegistrationType.Singleton);
        locator.RegisterType<SubObject1>(RegistrationType.Transient);
        locator.RegisterType<SubObject2>(RegistrationType.Transient);
        locator.RegisterType<SubObject3>(RegistrationType.Transient);
        locator.RegisterType<ComplexRoot1>(RegistrationType.Transient);
        locator.RegisterType<ComplexRoot2>(RegistrationType.Transient);
        locator.RegisterType<ComplexRoot3>(RegistrationType.Transient);
        _catel = locator;
    }

    [GlobalCleanup(Target = nameof(Catel))]
    public void CleanupCatel() => _catel.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Catel)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Catel() =>
        new(
            _catel.ResolveRequiredType<ComplexRoot1>(),
            _catel.ResolveRequiredType<ComplexRoot2>(),
            _catel.ResolveRequiredType<ComplexRoot3>());
}
