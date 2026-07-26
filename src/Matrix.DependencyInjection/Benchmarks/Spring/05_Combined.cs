using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Combined
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition(
            "combinedSingleton",
            new RootObjectDefinition(typeof(CombinedSingleton), true));
        factory.RegisterObjectDefinition(
            "combinedTransient",
            new RootObjectDefinition(typeof(CombinedTransient), false));
        factory.RegisterObjectDefinition("root1", Root(typeof(CombinedRoot1)));
        factory.RegisterObjectDefinition("root2", Root(typeof(CombinedRoot2)));
        factory.RegisterObjectDefinition("root3", Root(typeof(CombinedRoot3)));
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public BenchmarkRoots<CombinedRoot1, CombinedRoot2, CombinedRoot3> Spring()
    {
        var first = _spring.GetObject<CombinedRoot1>();
        var second = _spring.GetObject<CombinedRoot2>();
        var third = _spring.GetObject<CombinedRoot3>();
        Validate(LibraryCatalog.Spring, first, second, third);
        return new(first, second, third);
    }

    private static RootObjectDefinition Root(Type type)
    {
        var arguments = new ConstructorArgumentValues();
        arguments.AddGenericArgumentValue(new RuntimeObjectReference("combinedSingleton"));
        arguments.AddGenericArgumentValue(new RuntimeObjectReference("combinedTransient"));
        return new RootObjectDefinition(type, arguments, new()) { IsSingleton = false };
    }
}
