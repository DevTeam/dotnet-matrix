using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    public Dictionary<string, int>? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Dictionary(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
