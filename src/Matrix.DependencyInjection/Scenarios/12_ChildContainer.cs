namespace Matrix.DependencyInjection.Scenarios;

public interface IChildValue;
public sealed class ParentValue : IChildValue;
public sealed class ChildValue : IChildValue;

public sealed class ChildRoot(IChildValue value)
{
    public IChildValue Value { get; } = value;
}
