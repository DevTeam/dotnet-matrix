using Catel.IoC;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "Catel";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Registers and resolves one singleton and one transient service, exactly like the
    /// benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the singleton resolves to one instance and the transient to two.
    /// </summary>
    public static int Run()
    {
        using var locator = new ServiceLocator();
        locator.RegisterType<IProbeSingleton, ProbeSingleton>(RegistrationType.Singleton);
        locator.RegisterType<IProbeTransient, ProbeTransient>(RegistrationType.Transient);

        var s1 = locator.ResolveRequiredType<IProbeSingleton>();
        var s2 = locator.ResolveRequiredType<IProbeSingleton>();
        var t1 = locator.ResolveRequiredType<IProbeTransient>();
        var t2 = locator.ResolveRequiredType<IProbeTransient>();

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
