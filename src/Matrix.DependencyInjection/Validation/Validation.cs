using static Matrix.MatrixValidation;

namespace Matrix.DependencyInjection.Validation;

internal static class Validation
{
    private static int _pluginCount;

    public static void PropertyRoot(string library, IPropertyRoot root) =>
        Require(
            library,
            root.ServiceA is not null
            && root.ServiceB is not null
            && root.ServiceC is not null,
            "Property injection failed.");

    [Conditional("MATRIX_VALIDATION")]
    public static void PluginCreated() => _pluginCount++;

    public static void EnumerableRoots(
        string library,
        IEnumerableRoot first,
        IEnumerableRoot second,
        IEnumerableRoot third)
    {
        try
        {
            Require(library, _pluginCount == 0, "IEnumerable resolution is not lazy.");
            var firstItems = ValidatePlugins(library, first.Plugins);
            Require(library, _pluginCount == 5, "The first IEnumerable was not created during enumeration.");
            var secondItems = ValidatePlugins(library, second.Plugins);
            Require(library, _pluginCount == 10, "The second IEnumerable was not created during enumeration.");
            var thirdItems = ValidatePlugins(library, third.Plugins);
            Require(library, _pluginCount == 15, "The third IEnumerable was not created during enumeration.");
            Require(
                library,
                firstItems.Concat(secondItems).Concat(thirdItems).Distinct().Count() == 15,
                "IEnumerable reused transient plugin instances between roots.");
            var repeatedItems = ValidatePlugins(library, first.Plugins);
            Require(library, _pluginCount == 20, "Repeated IEnumerable enumeration did not create new plugins.");
            Require(
                library,
                firstItems.Concat(repeatedItems).Distinct().Count() == 10,
                "IEnumerable cached transient plugin instances between enumerations.");
        }
        finally
        {
            _pluginCount = 0;
        }
    }

    public static void ArrayRoots(
        string library,
        IArrayRoot first,
        IArrayRoot second,
        IArrayRoot third)
    {
        try
        {
            ValidatePlugins(library, first.Plugins);
            ValidatePlugins(library, second.Plugins);
            ValidatePlugins(library, third.Plugins);
            Require(library, _pluginCount == 15, "Array materialization did not create every plugin.");
        }
        finally
        {
            _pluginCount = 0;
        }
    }

    private static IPlugin[] ValidatePlugins(string library, IEnumerable<IPlugin> plugins)
    {
        var items = plugins.ToArray();
        Require(
            library,
            items.Length == 5
            && items.Select(plugin => plugin.GetType()).Distinct().Count() == 5,
            "Collection resolution failed.");
        return items;
    }
}
