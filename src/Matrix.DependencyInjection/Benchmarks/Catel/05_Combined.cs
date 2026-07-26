using Catel.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable RedundantArgumentDefaultValue

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private ServiceLocator _catel = null!;

    [GlobalSetup(Target = nameof(Catel))]
    public void SetupCatel()
    {
        var locator = new ServiceLocator();
        locator.RegisterType<ICombinedSingleton, CombinedSingleton>(RegistrationType.Singleton);
        locator.RegisterType<ICombinedTransient, CombinedTransient>(RegistrationType.Transient);
        locator.RegisterType<CombinedRoot1>(RegistrationType.Transient);
        locator.RegisterType<CombinedRoot2>(RegistrationType.Transient);
        locator.RegisterType<CombinedRoot3>(RegistrationType.Transient);
        _catel = locator;
    }

    [GlobalCleanup(Target = nameof(Catel))]
    public void CleanupCatel() => _catel.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Catel)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Catel()
    {
        var first = _catel.ResolveRequiredType<CombinedRoot1>();
        var second = _catel.ResolveRequiredType<CombinedRoot2>();
        var third = _catel.ResolveRequiredType<CombinedRoot3>();
        Validate(LibraryCatalog.Catel, first, second, third);
        return new(first, second, third);
    }
}
