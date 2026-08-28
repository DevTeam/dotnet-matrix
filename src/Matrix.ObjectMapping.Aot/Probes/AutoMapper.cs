using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace Matrix.ObjectMapping.Aot;

internal static class AotProbe
{
    public const string Library = "AutoMapper";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Builds a configuration in code, compiles it and maps one object, exactly like
    /// <c>AutoMapperFactory.CreateMapper</c> in the benchmarks, and checks the mapped result.
    /// </summary>
    public static int Run()
    {
        var configuration = new MapperConfiguration(
            expression => expression.CreateMap<ProbeSource, ProbeDestination>(),
            NullLoggerFactory.Instance);
        configuration.CompileMappings();
        var mapper = configuration.CreateMapper();

        var destination = mapper.Map<ProbeDestination>(new ProbeSource { Id = 7, Name = "probe" });
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
