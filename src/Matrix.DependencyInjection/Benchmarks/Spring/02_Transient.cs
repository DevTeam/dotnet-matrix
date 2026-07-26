using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Transient
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("transient1", new RootObjectDefinition(typeof(Transient1), false));
        factory.RegisterObjectDefinition("transient2", new RootObjectDefinition(typeof(Transient2), false));
        factory.RegisterObjectDefinition("transient3", new RootObjectDefinition(typeof(Transient3), false));
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public BenchmarkRoots<ITransient1, ITransient2, ITransient3> Spring()
    {
        var first = _spring.GetObject<ITransient1>();
        var second = _spring.GetObject<ITransient2>();
        var third = _spring.GetObject<ITransient3>();
        Validate(LibraryCatalog.Spring, first);
        return new(first, second, third);
    }
}
