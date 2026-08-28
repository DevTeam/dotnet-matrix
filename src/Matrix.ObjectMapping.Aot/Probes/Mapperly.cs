using Riok.Mapperly.Abstractions;

namespace Matrix.ObjectMapping.Aot;

// Mapperly generates its mapper implementation as a partial reopening of every enclosing type, so
// AotProbe itself must be declared partial for the generator to attach to ProbeMapper.
internal static partial class AotProbe
{
    public const string Library = "Mapperly";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Maps one object through Mapperly's generated code, exactly like <c>MapperlyMapper</c> in
    /// the benchmarks, and checks the mapped result. The mapper is declared right here: Mapperly's
    /// source generator only sees the one file that compiles for this probe.
    /// </summary>
    public static int Run()
    {
        var mapper = new ProbeMapper();
        var destination = mapper.Map(new ProbeSource { Id = 7, Name = "probe" });
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

    [Mapper]
    private partial class ProbeMapper
    {
        public partial ProbeDestination Map(ProbeSource source);
    }
}
