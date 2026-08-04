using System.Composition;
using Export = System.Composition.ExportAttribute;
// ReSharper disable UnusedMemberInSuper.Global

namespace Matrix.DependencyInjection.Scenarios;

[Export]
public sealed class PropertyServiceA;

[Export]
public sealed class PropertyServiceB;

[Export]
public sealed class PropertyServiceC;

public interface IPropertyRoot
{
    PropertyServiceA? ServiceA { get; set; }

    PropertyServiceB? ServiceB { get; set; }

    PropertyServiceC? ServiceC { get; set; }
}

[Export]
public sealed class PropertyRoot1 : IPropertyRoot
{
    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceA? ServiceA { get; set; }

    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceB? ServiceB { get; set; }

    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceC? ServiceC { get; set; }
}

[Export]
public sealed class PropertyRoot2 : IPropertyRoot
{
    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceA? ServiceA { get; set; }

    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceB? ServiceB { get; set; }

    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceC? ServiceC { get; set; }
}

[Export]
public sealed class PropertyRoot3 : IPropertyRoot
{
    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceA? ServiceA { get; set; }

    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceB? ServiceB { get; set; }

    [Import]
    [Pure.DI.Dependency]
    public PropertyServiceC? ServiceC { get; set; }
}
