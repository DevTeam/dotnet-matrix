using Maestro;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "Maestro";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Registers and resolves one singleton and one transient service, exactly like the
    /// benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the singleton resolves to one instance and the transient to two.
    /// </summary>
    public static int Run()
    {
        using var container = new Container(builder =>
        {
            builder.Add<IProbeSingleton>().Type<ProbeSingleton>().Singleton();
            builder.Add<IProbeTransient>().Type<ProbeTransient>().Transient();
        });

        var s1 = container.GetService<IProbeSingleton>();
        var s2 = container.GetService<IProbeSingleton>();
        var t1 = container.GetService<IProbeTransient>();
        var t2 = container.GetService<IProbeTransient>();

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

    private interface IProbeSingleton;

    private sealed class ProbeSingleton : IProbeSingleton;

    private interface IProbeTransient;

    private sealed class ProbeTransient : IProbeTransient;
}
