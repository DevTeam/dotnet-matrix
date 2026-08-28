using Pure.DI;
using System.Diagnostics;

namespace Matrix.DependencyInjection.Aot;

// Not static: Pure.DI generates the composition roots as instance members of the class that
// calls DI.Setup(), exactly like the benchmarks' Singleton/Transient classes do. Run() stays a
// static entry point, matching every other probe's Program.cs contract, and instantiates this
// class itself to reach the generated roots.
internal partial class AotProbe
{
    public const string Library = "Pure.DI";

    public const int ExpectedEvents = 2;

    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Singleton<ProbeSingleton>()
            .Root<IProbeSingleton>(nameof(SingletonRoot))
            .Transient<ProbeTransient>()
            .Root<IProbeTransient>(nameof(TransientRoot));

    /// <summary>
    /// Resolves one singleton and one transient composition root, exactly like the benchmarks'
    /// <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and checks that the
    /// singleton root resolves to one instance and the transient root to two.
    /// </summary>
    public static int Run()
    {
        var composition = new AotProbe();
        var s1 = composition.SingletonRoot;
        var s2 = composition.SingletonRoot;
        var t1 = composition.TransientRoot;
        var t2 = composition.TransientRoot;

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

    private sealed class ProbeSingleton : IProbeSingleton;

    public interface IProbeTransient;

    private sealed class ProbeTransient : IProbeTransient;
}
