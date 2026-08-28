using ServiceStack.Text;

namespace Matrix.JsonSerialization.Aot;

internal static class AotProbe
{
    public const string Library = "ServiceStack.Text";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Serializes and deserializes one object through ServiceStack.Text's reflection-based
    /// <c>JsonSerializer</c>, exactly like the benchmarks, and checks the result round-trips.
    /// </summary>
    public static int Run()
    {
        var input = new ProbeModel { Id = 7, Name = "probe", Active = true };
        var json = JsonSerializer.SerializeToString(input);
        var output = JsonSerializer.DeserializeFromString<ProbeModel>(json);
        return output is { Id: 7, Name: "probe", Active: true } ? 1 : 0;
    }

    private sealed class ProbeModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool Active { get; set; }
    }
}
