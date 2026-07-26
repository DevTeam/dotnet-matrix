using Catel.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable RedundantArgumentDefaultValue

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private ServiceLocator _catel = null!;

    [GlobalSetup(Target = nameof(Catel))]
    public void SetupCatel()
    {
        var locator = new ServiceLocator();
        locator.RegisterType<ISingleton1, Singleton1>(RegistrationType.Singleton);
        locator.RegisterType<ISingleton2, Singleton2>(RegistrationType.Singleton);
        locator.RegisterType<ISingleton3, Singleton3>(RegistrationType.Singleton);
        _catel = locator;
    }

    [GlobalCleanup(Target = nameof(Catel))]
    public void CleanupCatel() => _catel.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Catel)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Catel()
    {
        var first = _catel.ResolveRequiredType<ISingleton1>();
        var second = _catel.ResolveRequiredType<ISingleton2>();
        var third = _catel.ResolveRequiredType<ISingleton3>();
        Validate(LibraryCatalog.Catel, first, second, third);
        return new(first, second, third);
    }
}
