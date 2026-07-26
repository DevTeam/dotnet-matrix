using System.Composition;
using Export = System.Composition.ExportAttribute;

namespace Matrix.DependencyInjection.Scenarios;

public interface IArrayRoot
{
    IPlugin[] Plugins { get; }
}

[Export]
[method: ImportingConstructor]
public sealed class ArrayRoot1([ImportMany] IPlugin[] plugins) : IArrayRoot
{
    public IPlugin[] Plugins { get; } = plugins;
}

[Export]
[method: ImportingConstructor]
public sealed class ArrayRoot2([ImportMany] IPlugin[] plugins) : IArrayRoot
{
    public IPlugin[] Plugins { get; } = plugins;
}

[Export]
[method: ImportingConstructor]
public sealed class ArrayRoot3([ImportMany] IPlugin[] plugins) : IArrayRoot
{
    public IPlugin[] Plugins { get; } = plugins;
}
