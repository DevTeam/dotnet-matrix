using Spring.Objects.Factory.Support;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbe
{
    public const string Library = "Spring";

    public const int ExpectedEvents = 2;

    /// <summary>
    /// Registers and resolves one singleton and one prototype (transient) object, exactly like
    /// the benchmarks' <c>Singleton</c>/<c>Transient</c> scenarios minus the shared fixture, and
    /// checks that the singleton resolves to one instance and the prototype to two.
    /// </summary>
    public static int Run()
    {
        using var factory = new DefaultListableObjectFactory();
        factory.RegisterObjectDefinition("singleton", new RootObjectDefinition(typeof(ProbeSingleton), true));
        factory.RegisterObjectDefinition("transient", new RootObjectDefinition(typeof(ProbeTransient), false));

        var s1 = factory.GetObject<IProbeSingleton>("singleton");
        var s2 = factory.GetObject<IProbeSingleton>("singleton");
        var t1 = factory.GetObject<IProbeTransient>("transient");
        var t2 = factory.GetObject<IProbeTransient>("transient");

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
