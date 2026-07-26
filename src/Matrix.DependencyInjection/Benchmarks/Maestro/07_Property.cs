using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add<PropertyServiceA>().Self().Transient();
            builder.Add<PropertyServiceB>().Self().Transient();
            builder.Add<PropertyServiceC>().Self().Transient();
            builder.Add<PropertyRoot1>().Self()
                .SetProperty(root => root.ServiceA)
                .SetProperty(root => root.ServiceB)
                .SetProperty(root => root.ServiceC)
                .Transient();
            builder.Add<PropertyRoot2>().Self()
                .SetProperty(root => root.ServiceA)
                .SetProperty(root => root.ServiceB)
                .SetProperty(root => root.ServiceC)
                .Transient();
            builder.Add<PropertyRoot3>().Self()
                .SetProperty(root => root.ServiceA)
                .SetProperty(root => root.ServiceB)
                .SetProperty(root => root.ServiceC)
                .Transient();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Maestro()
    {
        var first = _maestro.GetService<PropertyRoot1>();
        var second = _maestro.GetService<PropertyRoot2>();
        var third = _maestro.GetService<PropertyRoot3>();
        Validate(LibraryCatalog.Maestro, first, second, third);
        return new(first, second, third);
    }
}
