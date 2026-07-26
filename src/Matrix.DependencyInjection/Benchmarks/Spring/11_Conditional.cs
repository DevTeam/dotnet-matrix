using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("service1", new RootObjectDefinition(typeof(ConditionalService1), false));
        factory.RegisterObjectDefinition("service2", new RootObjectDefinition(typeof(ConditionalService2), false));
        factory.RegisterObjectDefinition("service3", new RootObjectDefinition(typeof(ConditionalService3), false));
        factory.RegisterObjectDefinition("root1", Definition(typeof(ConditionalRoot1), "service1"));
        factory.RegisterObjectDefinition("root2", Definition(typeof(ConditionalRoot2), "service2"));
        factory.RegisterObjectDefinition("root3", Definition(typeof(ConditionalRoot3), "service3"));
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Spring()
    {
        var first = _spring.GetObject<ConditionalRoot1>();
        var second = _spring.GetObject<ConditionalRoot2>();
        var third = _spring.GetObject<ConditionalRoot3>();
        Validate(LibraryCatalog.Spring, first, second, third);
        return new(first, second, third);
    }

    private static RootObjectDefinition Definition(Type type, string reference)
    {
        var arguments = new ConstructorArgumentValues();
        arguments.AddGenericArgumentValue(new RuntimeObjectReference(reference));
        return new RootObjectDefinition(type, arguments, new()) { IsSingleton = false };
    }
}
