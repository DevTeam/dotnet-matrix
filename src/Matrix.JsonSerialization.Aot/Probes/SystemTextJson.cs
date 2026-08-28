using System.Text.Json;

namespace Matrix.JsonSerialization.Aot;

internal static class AotProbe
{
    public const string Library = "System.Text.Json";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Serializes and deserializes one object through the reflection-based path -
    /// <c>new JsonSerializerOptions()</c> with no source-generated <c>JsonTypeInfo</c>, exactly
    /// like <c>JsonConfiguration.SystemTextDefault</c> in the benchmarks - and checks the result
    /// round-trips. This is the path source generation exists to avoid, so what is probed is
    /// System.Text.Json's own reflection fallback under Native AOT, not its source generator.
    /// </summary>
    public static int Run()
    {
        var options = new JsonSerializerOptions();
        var input = new ProbeModel { Id = 7, Name = "probe", Active = true };
        var json = JsonSerializer.Serialize(input, options);
        var output = JsonSerializer.Deserialize<ProbeModel>(json, options);
        return output is { Id: 7, Name: "probe", Active: true } ? 1 : 0;
    }

    private sealed class ProbeModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool Active { get; set; }
    }
}
