// ReSharper disable RedundantUsingDirective
using System.Composition;
using Export = System.Composition.ExportAttribute;

namespace Matrix.DependencyInjection.Scenarios;

public interface IPlugin;

[Export(typeof(IPlugin))]
public sealed class Plugin1 : IPlugin
{
    public Plugin1() => Validation.Validation.PluginCreated();
}

[Export(typeof(IPlugin))]
public sealed class Plugin2 : IPlugin
{
    public Plugin2() => Validation.Validation.PluginCreated();
}

[Export(typeof(IPlugin))]
public sealed class Plugin3 : IPlugin
{
    public Plugin3() => Validation.Validation.PluginCreated();
}

[Export(typeof(IPlugin))]
public sealed class Plugin4 : IPlugin
{
    public Plugin4() => Validation.Validation.PluginCreated();
}

[Export(typeof(IPlugin))]
public sealed class Plugin5 : IPlugin
{
    public Plugin5() => Validation.Validation.PluginCreated();
}

public interface IEnumerableRoot
{
    IEnumerable<IPlugin> Plugins { get; }
}

public sealed class EnumerableRoot1(IEnumerable<IPlugin> plugins) : IEnumerableRoot
{
    public IEnumerable<IPlugin> Plugins { get; } = plugins;
}

public sealed class EnumerableRoot2(IEnumerable<IPlugin> plugins) : IEnumerableRoot
{
    public IEnumerable<IPlugin> Plugins { get; } = plugins;
}

public sealed class EnumerableRoot3(IEnumerable<IPlugin> plugins) : IEnumerableRoot
{
    public IEnumerable<IPlugin> Plugins { get; } = plugins;
}
