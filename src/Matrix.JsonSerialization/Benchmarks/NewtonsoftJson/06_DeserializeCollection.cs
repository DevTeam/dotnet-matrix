using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.CollectionJson)]
    public SimpleModel[]? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<SimpleModel[]>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Collection(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
