using Newtonsoft.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.EnumJson)]
    public EnumModel? NewtonsoftJson()
    {
        var json = JsonConvert.SerializeObject(_input, JsonConfiguration.NewtonsoftEnum);
        SerializationChecks.Json(
            LibraryCatalog.NewtonsoftJson,
            json,
            SerializationData.EnumJson);
        var result = JsonConvert.DeserializeObject<EnumModel>(
            json,
            JsonConfiguration.NewtonsoftEnum);
        SerializationChecks.Enum(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
