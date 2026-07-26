using Ninject;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Generics
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Bind(typeof(IGenericService<>)).To(typeof(GenericService<>)).InTransientScope();
        kernel.Bind(typeof(GenericRoot<>)).ToSelf().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<GenericRoot<int>, GenericRoot<float>, GenericRoot<object>> Ninject() =>
        new(
            _ninject.Get<GenericRoot<int>>(),
            _ninject.Get<GenericRoot<float>>(),
            _ninject.Get<GenericRoot<object>>());
}
