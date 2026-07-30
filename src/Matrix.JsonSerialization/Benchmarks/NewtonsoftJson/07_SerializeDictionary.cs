using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    public string NewtonsoftJson()
    {
        var json = JsonConvert.SerializeObject(_input, JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Json(
            LibraryCatalog.NewtonsoftJson,
            json,
            SerializationData.DictionaryJson);
        return json;
    }
}
