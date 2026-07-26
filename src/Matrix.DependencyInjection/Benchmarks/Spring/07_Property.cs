using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("serviceA", new RootObjectDefinition(typeof(PropertyServiceA), false));
        factory.RegisterObjectDefinition("serviceB", new RootObjectDefinition(typeof(PropertyServiceB), false));
        factory.RegisterObjectDefinition("serviceC", new RootObjectDefinition(typeof(PropertyServiceC), false));
        factory.RegisterObjectDefinition("root1", Autowired(typeof(PropertyRoot1)));
        factory.RegisterObjectDefinition("root2", Autowired(typeof(PropertyRoot2)));
        factory.RegisterObjectDefinition("root3", Autowired(typeof(PropertyRoot3)));
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Spring()
    {
        var first = _spring.GetObject<PropertyRoot1>();
        var second = _spring.GetObject<PropertyRoot2>();
        var third = _spring.GetObject<PropertyRoot3>();
        Validate(LibraryCatalog.Spring, first, second, third);
        return new(first, second, third);
    }

    private static RootObjectDefinition Autowired(Type type) =>
        new(type, AutoWiringMode.ByType) { IsSingleton = false };
}
