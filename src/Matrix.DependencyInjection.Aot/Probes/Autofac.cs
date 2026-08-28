using Autofac;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "Autofac";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Registers and resolves one singleton and one transient service, exactly like the
    /// benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the singleton resolves to one instance and the transient to two.
    /// </summary>
    public static int Run()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ProbeSingleton>().As<IProbeSingleton>().SingleInstance();
        builder.RegisterType<ProbeTransient>().As<IProbeTransient>();
        using var container = builder.Build();

        var s1 = container.Resolve<IProbeSingleton>();
        var s2 = container.Resolve<IProbeSingleton>();
        var t1 = container.Resolve<IProbeTransient>();
        var t2 = container.Resolve<IProbeTransient>();

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
