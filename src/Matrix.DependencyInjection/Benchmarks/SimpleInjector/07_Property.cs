using SimpleInjector;
using SimpleInjector.Advanced;
using System.Reflection;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private Container _simpleInjector = null!;

    [GlobalSetup(Target = nameof(SimpleInjector))]
    public void SetupSimpleInjector()
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Options.PropertySelectionBehavior = new PropertySelectionBehavior();
        container.Register<PropertyServiceA>();
        container.Register<PropertyServiceB>();
        container.Register<PropertyServiceC>();
        container.Register<PropertyRoot1>();
        container.Register<PropertyRoot2>();
        container.Register<PropertyRoot3>();
        _simpleInjector = container;
    }

    [GlobalCleanup(Target = nameof(SimpleInjector))]
    public void CleanupSimpleInjector() => _simpleInjector.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SimpleInjector)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> SimpleInjector()
    {
        var first = _simpleInjector.GetInstance<PropertyRoot1>();
        var second = _simpleInjector.GetInstance<PropertyRoot2>();
        var third = _simpleInjector.GetInstance<PropertyRoot3>();
        Validate(LibraryCatalog.SimpleInjector, first, second, third);
        return new(first, second, third);
    }

    private sealed class PropertySelectionBehavior : IPropertySelectionBehavior
    {
        public bool SelectProperty(Type implementationType, PropertyInfo propertyInfo) =>
            propertyInfo.PropertyType == typeof(PropertyServiceA)
            || propertyInfo.PropertyType == typeof(PropertyServiceB)
            || propertyInfo.PropertyType == typeof(PropertyServiceC);
    }
}
