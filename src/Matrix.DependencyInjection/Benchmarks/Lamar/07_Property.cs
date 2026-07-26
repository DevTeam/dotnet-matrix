using Lamar;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private Container _lamar = null!;

    [GlobalSetup(Target = nameof(Lamar))]
    public void SetupLamar() =>
        _lamar = Container.For(registry =>
        {
            registry.Policies.SetAllProperties(convention =>
            {
                convention.OfType<PropertyServiceA>();
                convention.OfType<PropertyServiceB>();
                convention.OfType<PropertyServiceC>();
            });
            registry.For<PropertyServiceA>().Use<PropertyServiceA>().Transient();
            registry.For<PropertyServiceB>().Use<PropertyServiceB>().Transient();
            registry.For<PropertyServiceC>().Use<PropertyServiceC>().Transient();
            registry.For<PropertyRoot1>().Use<PropertyRoot1>().Transient();
            registry.For<PropertyRoot2>().Use<PropertyRoot2>().Transient();
            registry.For<PropertyRoot3>().Use<PropertyRoot3>().Transient();
        });

    [GlobalCleanup(Target = nameof(Lamar))]
    public void CleanupLamar() => _lamar.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Lamar)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Lamar()
    {
        var first = _lamar.GetInstance<PropertyRoot1>();
        var second = _lamar.GetInstance<PropertyRoot2>();
        var third = _lamar.GetInstance<PropertyRoot3>();
        Validate(LibraryCatalog.Lamar, first, second, third);
        return new(first, second, third);
    }
}
