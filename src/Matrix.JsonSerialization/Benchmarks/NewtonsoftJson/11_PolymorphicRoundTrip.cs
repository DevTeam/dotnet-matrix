using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class PolymorphicRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.PolymorphicJson)]
    public ZooModel? NewtonsoftJson()
    {
        var json = JsonConvert.SerializeObject(_input, JsonConfiguration.NewtonsoftPolymorphic);
        SerializationChecks.Json(
            LibraryCatalog.NewtonsoftJson,
            json,
            SerializationData.PolymorphicJson);
        var result = JsonConvert.DeserializeObject<ZooModel>(
            json,
            JsonConfiguration.NewtonsoftPolymorphic);
        SerializationChecks.Zoo(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
