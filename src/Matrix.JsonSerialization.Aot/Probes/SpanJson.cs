using SpanJson;

namespace Matrix.JsonSerialization.Aot;

internal static class AotProbe
{
    public const string Library = "SpanJson";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Serializes and deserializes one object through SpanJson's default reflection-created
    /// resolver, exactly like the benchmarks (<c>JsonSerializer.Generic.Utf16</c> with no custom
    /// resolver), and checks the result round-trips.
    /// </summary>
    public static int Run()
    {
        var input = new ProbeModel { Id = 7, Name = "probe", Active = true };
        var json = JsonSerializer.Generic.Utf16.Serialize(input);
        var output = JsonSerializer.Generic.Utf16.Deserialize<ProbeModel>(json);
        return output is { Id: 7, Name: "probe", Active: true } ? 1 : 0;
    }

    private sealed class ProbeModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool Active { get; set; }
    }
}
