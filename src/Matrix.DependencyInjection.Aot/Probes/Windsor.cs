using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "Windsor";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Registers and resolves one singleton and one transient service, exactly like the
    /// benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the singleton resolves to one instance and the transient to two.
    /// </summary>
    public static int Run()
    {
        using var container = new WindsorContainer();
        container.Register(
            Component.For<IProbeSingleton>().ImplementedBy<ProbeSingleton>().LifestyleSingleton(),
            Component.For<IProbeTransient>().ImplementedBy<ProbeTransient>().LifestyleTransient());

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

    public interface IProbeSingleton;

    public sealed class ProbeSingleton : IProbeSingleton;

    public interface IProbeTransient;

    public sealed class ProbeTransient : IProbeTransient;
}
