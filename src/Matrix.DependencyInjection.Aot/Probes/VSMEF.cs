using Microsoft.VisualStudio.Composition;
using System.Composition;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "VS.MEF";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Composes one shared (singleton) part and one non-shared (transient) part through VS MEF's
    /// attributed part discovery, exactly like the benchmarks' <c>Singleton</c>/<c>Transient</c>
    /// scenarios minus the shared fixture, and checks that the shared part resolves to one
    /// instance and the non-shared part to two.
    /// </summary>
    public static int Run()
    {
        var discovery = new AttributedPartDiscovery(Resolver.DefaultInstance, true);
        var catalog = ComposableCatalog.Create(Resolver.DefaultInstance)
            .AddParts(new[] { typeof(ProbeSingleton), typeof(ProbeTransient) }.Select(discovery.CreatePart)!);
        using var container = CompositionConfiguration.Create(catalog)
            .CreateExportProviderFactory()
            .CreateExportProvider();

        var s1 = container.GetExportedValue<IProbeSingleton>();
        var s2 = container.GetExportedValue<IProbeSingleton>();
        var t1 = container.GetExportedValue<IProbeTransient>();
        var t2 = container.GetExportedValue<IProbeTransient>();

        var events = 0;
        if (ReferenceEquals(s1, s2))
        {
            events++;
        }

        if (!ReferenceEquals(t1, t2))
        {
            events++;
        }

        return events;
    }

    public interface IProbeSingleton;

    [Export(typeof(IProbeSingleton))]
    [Shared]
    public sealed class ProbeSingleton : IProbeSingleton;

    public interface IProbeTransient;

    [Export(typeof(IProbeTransient))]
    public sealed class ProbeTransient : IProbeTransient;
}
