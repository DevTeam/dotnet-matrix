using Jab;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "Jab";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Registers and resolves one singleton and one transient service, exactly like the
    /// benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the singleton resolves to one instance and the transient to two.
    /// Jab generates the whole container at compile time, so nothing here relies on reflection.
    /// </summary>
    public static int Run()
    {
        var provider = new JabAotProbeComposition();

        var s1 = provider.GetService<IJabProbeSingleton>();
        var s2 = provider.GetService<IJabProbeSingleton>();
        var t1 = provider.GetService<IJabProbeTransient>();
        var t2 = provider.GetService<IJabProbeTransient>();

        if (provider is IDisposable disposable)
        {
            disposable.Dispose();
        }

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
}

internal interface IJabProbeSingleton;

internal sealed class JabProbeSingleton : IJabProbeSingleton;

internal interface IJabProbeTransient;

internal sealed class JabProbeTransient : IJabProbeTransient;

[ServiceProvider]
[Singleton(typeof(IJabProbeSingleton), typeof(JabProbeSingleton))]
[Transient(typeof(IJabProbeTransient), typeof(JabProbeTransient))]
internal partial class JabAotProbeComposition;
