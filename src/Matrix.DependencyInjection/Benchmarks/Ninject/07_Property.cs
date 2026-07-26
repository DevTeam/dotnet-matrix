using System.Reflection;
using Ninject;
using Ninject.Components;
using Ninject.Selection.Heuristics;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Property
{
    private StandardKernel _ninject = null!;

    [GlobalSetup(Target = nameof(Ninject))]
    public void SetupNinject()
    {
        var kernel = new StandardKernel();
        kernel.Components.Add<IInjectionHeuristic, PropertyServiceHeuristic>();
        kernel.Bind<PropertyServiceA>().ToSelf().InTransientScope();
        kernel.Bind<PropertyServiceB>().ToSelf().InTransientScope();
        kernel.Bind<PropertyServiceC>().ToSelf().InTransientScope();
        kernel.Bind<PropertyRoot1>().ToSelf().InTransientScope();
        kernel.Bind<PropertyRoot2>().ToSelf().InTransientScope();
        kernel.Bind<PropertyRoot3>().ToSelf().InTransientScope();
        _ninject = kernel;
    }

    [GlobalCleanup(Target = nameof(Ninject))]
    public void CleanupNinject() => _ninject.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ninject)]
    public BenchmarkRoots<PropertyRoot1, PropertyRoot2, PropertyRoot3> Ninject()
    {
        var first = _ninject.Get<PropertyRoot1>();
        var second = _ninject.Get<PropertyRoot2>();
        var third = _ninject.Get<PropertyRoot3>();
        Validate(LibraryCatalog.Ninject, first, second, third);
        return new(first, second, third);
    }

    // Ninject selects injected members through injection heuristics instead of a convention switch.
    private sealed class PropertyServiceHeuristic : NinjectComponent, IInjectionHeuristic
    {
        public bool ShouldInject(MemberInfo member) =>
            member is PropertyInfo property
            && (property.PropertyType == typeof(PropertyServiceA)
                || property.PropertyType == typeof(PropertyServiceB)
                || property.PropertyType == typeof(PropertyServiceC));
    }
}
