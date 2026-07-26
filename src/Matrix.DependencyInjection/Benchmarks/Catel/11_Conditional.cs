using Catel.IoC;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private ServiceLocator _catel = null!;

    [GlobalSetup(Target = nameof(Catel))]
    public void SetupCatel()
    {
        var locator = new ServiceLocator();
        locator.RegisterTypeWithTag<IConditionalService, ConditionalService1>(
            "1",
            RegistrationType.Transient);
        locator.RegisterTypeWithTag<IConditionalService, ConditionalService2>(
            "2",
            RegistrationType.Transient);
        locator.RegisterTypeWithTag<IConditionalService, ConditionalService3>(
            "3",
            RegistrationType.Transient);
        locator.RegisterType<ConditionalRoot1>(
            (_, _) => new ConditionalRoot1(locator.ResolveRequiredType<IConditionalService>("1")),
            RegistrationType.Transient);
        locator.RegisterType<ConditionalRoot2>(
            (_, _) => new ConditionalRoot2(locator.ResolveRequiredType<IConditionalService>("2")),
            RegistrationType.Transient);
        locator.RegisterType<ConditionalRoot3>(
            (_, _) => new ConditionalRoot3(locator.ResolveRequiredType<IConditionalService>("3")),
            RegistrationType.Transient);
        _catel = locator;
    }

    [GlobalCleanup(Target = nameof(Catel))]
    public void CleanupCatel() => _catel.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Catel)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Catel()
    {
        var first = _catel.ResolveRequiredType<ConditionalRoot1>();
        var second = _catel.ResolveRequiredType<ConditionalRoot2>();
        var third = _catel.ResolveRequiredType<ConditionalRoot3>();
        Validate(LibraryCatalog.Catel, first, second, third);
        return new(first, second, third);
    }
}
