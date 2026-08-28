using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Matrix.JsonSerialization.Aot;

internal static class AotProbe
{
    public const string Library = "Newtonsoft.Json";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Serializes and deserializes one object through Newtonsoft's reflection-based contract
    /// resolver, exactly like <c>JsonConfiguration.NewtonsoftDefault</c> in the benchmarks, and
    /// checks the result round-trips.
    /// </summary>
    public static int Run()
    {
        var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        var input = new ProbeModel { Id = 7, Name = "probe", Active = true };
        var json = JsonConvert.SerializeObject(input, settings);
        var output = JsonConvert.DeserializeObject<ProbeModel>(json, settings);
        return output is { Id: 7, Name: "probe", Active: true } ? 1 : 0;
    }

    private sealed class ProbeModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool Active { get; set; }
    }
}
