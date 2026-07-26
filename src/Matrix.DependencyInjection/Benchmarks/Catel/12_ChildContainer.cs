using Catel.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class ChildContainer
{
    private ServiceLocator _catel = null!;

    [GlobalSetup(Target = nameof(Catel))]
    public void SetupCatel()
    {
        var locator = new ServiceLocator();
        locator.RegisterType<IChildValue, ParentValue>(RegistrationType.Transient);
        locator.RegisterType<ChildRoot>(RegistrationType.Transient);
        _catel = locator;
    }

    [GlobalCleanup(Target = nameof(Catel))]
    public void CleanupCatel() => _catel.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Catel)]
    public BenchmarkRoots<ChildRoot, ChildRoot> Catel()
    {
        var parent = _catel.ResolveRequiredType<ChildRoot>();
        using var child = new ServiceLocator(_catel);
        child.RegisterType<IChildValue, ChildValue>(RegistrationType.Transient);
        var root = child.ResolveRequiredType<ChildRoot>();
        Validate(LibraryCatalog.Catel, parent, root);
        return new(parent, root);
    }
}
