using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class PrepareAndRegisterAndSimpleResolve
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Spring)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public ISingleton1 Spring()
    {
        using var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("singleton1", new RootObjectDefinition(typeof(Singleton1), true));
        factory.RegisterObjectDefinition("singleton2", new RootObjectDefinition(typeof(Singleton2), true));
        factory.RegisterObjectDefinition("singleton3", new RootObjectDefinition(typeof(Singleton3), true));
        factory.RegisterObjectDefinition("transient1", new RootObjectDefinition(typeof(Transient1), false));
        factory.RegisterObjectDefinition("transient2", new RootObjectDefinition(typeof(Transient2), false));
        factory.RegisterObjectDefinition("transient3", new RootObjectDefinition(typeof(Transient3), false));
        factory.RegisterObjectDefinition("first", new RootObjectDefinition(typeof(FirstService), true));
        factory.RegisterObjectDefinition("second", new RootObjectDefinition(typeof(SecondService), true));
        factory.RegisterObjectDefinition("third", new RootObjectDefinition(typeof(ThirdService), true));
        factory.RegisterObjectDefinition("sub1", Definition(typeof(SubObject1), "first"));
        factory.RegisterObjectDefinition("sub2", Definition(typeof(SubObject2), "first", "second"));
        factory.RegisterObjectDefinition("sub3", Definition(typeof(SubObject3), "sub1", "sub2", "third"));
        factory.RegisterObjectDefinition("root1", Definition(typeof(ComplexRoot1), "sub3"));
        return factory.GetObject<ISingleton1>();
    }

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
