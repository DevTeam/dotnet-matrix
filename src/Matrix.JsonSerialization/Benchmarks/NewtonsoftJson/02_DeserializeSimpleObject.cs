using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<SimpleModel>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Simple(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
