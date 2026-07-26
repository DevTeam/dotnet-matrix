using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Array
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("plugin1", new RootObjectDefinition(typeof(Plugin1), false));
        factory.RegisterObjectDefinition("plugin2", new RootObjectDefinition(typeof(Plugin2), false));
        factory.RegisterObjectDefinition("plugin3", new RootObjectDefinition(typeof(Plugin3), false));
        factory.RegisterObjectDefinition("plugin4", new RootObjectDefinition(typeof(Plugin4), false));
        factory.RegisterObjectDefinition("plugin5", new RootObjectDefinition(typeof(Plugin5), false));
        factory.RegisterObjectDefinition("root1", Definition(typeof(ArrayRoot1)));
        factory.RegisterObjectDefinition("root2", Definition(typeof(ArrayRoot2)));
        factory.RegisterObjectDefinition("root3", Definition(typeof(ArrayRoot3)));
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public BenchmarkRoots<ArrayRoot1, ArrayRoot2, ArrayRoot3> Spring()
    {
        var first = _spring.GetObject<ArrayRoot1>();
        var second = _spring.GetObject<ArrayRoot2>();
        var third = _spring.GetObject<ArrayRoot3>();
        Validate(LibraryCatalog.Spring, first, second, third);
        return new(first, second, third);
    }

    private static RootObjectDefinition Definition(Type type)
    {
        var plugins = new ManagedList { ElementTypeName = typeof(IPlugin).AssemblyQualifiedName };
        for (var index = 1; index <= 5; index++)
        {
            plugins.Add(new RuntimeObjectReference($"plugin{index}"));
        }

        var arguments = new ConstructorArgumentValues();
        arguments.AddGenericArgumentValue(plugins);
        return new RootObjectDefinition(type, arguments, new()) { IsSingleton = false };
    }
}
