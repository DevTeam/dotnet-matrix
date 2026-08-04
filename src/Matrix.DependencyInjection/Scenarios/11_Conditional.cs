using System.Composition;
using Export = System.Composition.ExportAttribute;
// ReSharper disable UnusedMemberInSuper.Global

namespace Matrix.DependencyInjection.Scenarios;

public interface IConditionalService;

[Export("1", typeof(IConditionalService))]
public sealed class ConditionalService1 : IConditionalService;

[Export("2", typeof(IConditionalService))]
public sealed class ConditionalService2 : IConditionalService;

[Export("3", typeof(IConditionalService))]
public sealed class ConditionalService3 : IConditionalService;

public interface IConditionalRoot
{
    IConditionalService Service { get; }
}

[Export]
[method: ImportingConstructor]
public sealed class ConditionalRoot1(
    [Tag("1")] [Import("1")] IConditionalService service) : IConditionalRoot
{
    public IConditionalService Service { get; } = service;
}

[Export]
[method: ImportingConstructor]
public sealed class ConditionalRoot2(
    [Tag("2")] [Import("2")] IConditionalService service) : IConditionalRoot
{
    public IConditionalService Service { get; } = service;
}

[Export]
[method: ImportingConstructor]
public sealed class ConditionalRoot3(
    [Tag("3")] [Import("3")] IConditionalService service) : IConditionalRoot
{
    public IConditionalService Service { get; } = service;
}
