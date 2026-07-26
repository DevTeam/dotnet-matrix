using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Singleton
{
    private DefaultListableObjectFactory _spring = null!;

    [GlobalSetup(Target = nameof(Spring))]
    public void SetupSpring()
    {
        var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("singleton1", new RootObjectDefinition(typeof(Singleton1), true));
        factory.RegisterObjectDefinition("singleton2", new RootObjectDefinition(typeof(Singleton2), true));
        factory.RegisterObjectDefinition("singleton3", new RootObjectDefinition(typeof(Singleton3), true));
        _spring = factory;
    }

    [GlobalCleanup(Target = nameof(Spring))]
    public void CleanupSpring() => _spring.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    public BenchmarkRoots<ISingleton1, ISingleton2, ISingleton3> Spring()
    {
        var first = _spring.GetObject<ISingleton1>();
        var second = _spring.GetObject<ISingleton2>();
        var third = _spring.GetObject<ISingleton3>();
        Validate(LibraryCatalog.Spring, first, second, third);
        return new(first, second, third);
    }
}
