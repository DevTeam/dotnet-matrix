using Catel.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private ServiceLocator _catel = null!;

    [GlobalSetup(Target = nameof(Catel))]
    public void SetupCatel()
    {
        var locator = new ServiceLocator();
        locator.RegisterType<ITransient1, Transient1>(RegistrationType.Transient);
        locator.RegisterType<ITransient2, Transient2>(RegistrationType.Transient);
        locator.RegisterType<ITransient3, Transient3>(RegistrationType.Transient);
        _catel = locator;
    }

    [GlobalCleanup(Target = nameof(Catel))]
    public void CleanupCatel() => _catel.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Catel)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Catel()
    {
        var first = _catel.ResolveRequiredType<ITransient1>();
        var second = _catel.ResolveRequiredType<ITransient2>();
        var third = _catel.ResolveRequiredType<ITransient3>();
        Validate(LibraryCatalog.Catel, first);
        return new(first, second, third);
    }
}
