using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.NestedJson)]
    public string NewtonsoftJson()
    {
        var json = JsonConvert.SerializeObject(_input, JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Json(
            LibraryCatalog.NewtonsoftJson,
            json,
            SerializationData.NestedJson);
        return json;
    }
}
