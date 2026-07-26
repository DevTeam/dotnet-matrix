using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Complex
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("first", new RootObjectDefinition(typeof(FirstService), true));
        factory.RegisterObjectDefinition("second", new RootObjectDefinition(typeof(SecondService), true));
        factory.RegisterObjectDefinition("third", new RootObjectDefinition(typeof(ThirdService), true));
        factory.RegisterObjectDefinition("sub1", Definition(typeof(SubObject1), "first"));
        factory.RegisterObjectDefinition("sub2", Definition(typeof(SubObject2), "first", "second"));
        factory.RegisterObjectDefinition("sub3", Definition(typeof(SubObject3), "sub1", "sub2", "third"));
        factory.RegisterObjectDefinition("root1", Definition(typeof(ComplexRoot1), "sub3"));
        factory.RegisterObjectDefinition("root2", Definition(typeof(ComplexRoot2), "sub3"));
        factory.RegisterObjectDefinition("root3", Definition(typeof(ComplexRoot3), "sub3"));
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public BenchmarkRoots<ComplexRoot1, ComplexRoot2, ComplexRoot3> Spring() =>
        new(
            _spring.GetObject<ComplexRoot1>(),
            _spring.GetObject<ComplexRoot2>(),
            _spring.GetObject<ComplexRoot3>());

    private static RootObjectDefinition Definition(Type type, params string[] references)
    {
        var arguments = new ConstructorArgumentValues();
        foreach (var reference in references)
        {
            arguments.AddGenericArgumentValue(new RuntimeObjectReference(reference));
        }

        return new RootObjectDefinition(type, arguments, new()) { IsSingleton = false };
    }
}
