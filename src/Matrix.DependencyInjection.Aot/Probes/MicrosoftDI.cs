using Microsoft.Extensions.DependencyInjection;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "Microsoft.DI";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Registers and resolves one singleton and one transient service, exactly like the
    /// benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the singleton resolves to one instance and the transient to two.
    /// </summary>
    public static int Run()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IProbeSingleton, ProbeSingleton>();
        services.AddTransient<IProbeTransient, ProbeTransient>();
        using var provider = services.BuildServiceProvider();

        var s1 = provider.GetRequiredService<IProbeSingleton>();
        var s2 = provider.GetRequiredService<IProbeSingleton>();
        var t1 = provider.GetRequiredService<IProbeTransient>();
        var t2 = provider.GetRequiredService<IProbeTransient>();

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
