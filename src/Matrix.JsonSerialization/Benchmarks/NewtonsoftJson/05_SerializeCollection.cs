using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.CollectionJson)]
    public string NewtonsoftJson()
    {
        var json = JsonConvert.SerializeObject(_input, JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Json(
            LibraryCatalog.NewtonsoftJson,
            json,
            SerializationData.CollectionJson);
        return json;
    }
}
