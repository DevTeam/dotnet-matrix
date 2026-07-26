using System.Composition;
using Export = System.Composition.ExportAttribute;

namespace Matrix.DependencyInjection.Scenarios;

public interface IFirstService;
public interface ISecondService;
public interface IThirdService;

[Export(typeof(IFirstService))]
[Shared]
public sealed class FirstService : IFirstService;

[Export(typeof(ISecondService))]
[Shared]
public sealed class SecondService : ISecondService;

[Export(typeof(IThirdService))]
[Shared]
public sealed class ThirdService : IThirdService;

[Export]
[method: ImportingConstructor]
public sealed class SubObject1(IFirstService first)
{
    public IFirstService First { get; } = first;
}

[Export]
[method: ImportingConstructor]
public sealed class SubObject2(
    IFirstService first,
    ISecondService second)
{
    public IFirstService First { get; } = first;

    public ISecondService Second { get; } = second;
}

[Export]
[method: ImportingConstructor]
public sealed class SubObject3(
    SubObject1 first,
    SubObject2 second,
    IThirdService third)
{
    public SubObject1 First { get; } = first;

    public SubObject2 Second { get; } = second;

    public IThirdService Third { get; } = third;
}

public interface IComplexRoot
{
    SubObject3 Value { get; }
}

[Export]
[method: ImportingConstructor]
public sealed class ComplexRoot1(SubObject3 value) : IComplexRoot
{
    public SubObject3 Value { get; } = value;
}

[Export]
[method: ImportingConstructor]
public sealed class ComplexRoot2(SubObject3 value) : IComplexRoot
{
    public SubObject3 Value { get; } = value;
}

[Export]
[method: ImportingConstructor]
public sealed class ComplexRoot3(SubObject3 value) : IComplexRoot
{
    public SubObject3 Value { get; } = value;
}
