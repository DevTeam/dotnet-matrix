using Mapster;

namespace Matrix.ObjectMapping.Aot;

internal static class AotProbe
{
    public const string Library = "Mapster";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Builds a configuration in code, compiles it and maps one object, exactly like
    /// <c>MapsterFactory.CreateConfiguration</c> in the benchmarks, and checks the mapped result.
    /// </summary>
    public static int Run()
    {
        var configuration = new TypeAdapterConfig();
        configuration.NewConfig<ProbeSource, ProbeDestination>();
        configuration.Compile();

        var destination = new ProbeSource { Id = 7, Name = "probe" }
            .Adapt<ProbeDestination>(configuration);
        return destination is { Id: 7, Name: "probe" } ? 1 : 0;
    }

    private sealed class ProbeSource
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;
    }

    private sealed class ProbeDestination
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
