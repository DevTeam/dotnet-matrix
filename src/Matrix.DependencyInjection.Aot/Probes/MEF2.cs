using System.Composition;
using System.Composition.Hosting;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "MEF2";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Composes one shared (singleton) part and one non-shared (transient) part, exactly like the
    /// benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the shared part resolves to one instance and the non-shared part to two.
    /// </summary>
    public static int Run()
    {
        using var container = new ContainerConfiguration()
            .WithParts(typeof(ProbeSingleton), typeof(ProbeTransient))
            .CreateContainer();

        var s1 = container.GetExport<IProbeSingleton>();
        var s2 = container.GetExport<IProbeSingleton>();
        var t1 = container.GetExport<IProbeTransient>();
        var t2 = container.GetExport<IProbeTransient>();

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
