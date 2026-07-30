using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class CustomConverterRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.IdentifierJson)]
    public IdentifierModel? NewtonsoftJson()
    {
        var json = JsonConvert.SerializeObject(_input, JsonConfiguration.NewtonsoftIdentifier);
        SerializationChecks.Json(
            LibraryCatalog.NewtonsoftJson,
            json,
            SerializationData.IdentifierJson);
        var result = JsonConvert.DeserializeObject<IdentifierModel>(
            json,
            JsonConfiguration.NewtonsoftIdentifier);
        SerializationChecks.Identifier(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
