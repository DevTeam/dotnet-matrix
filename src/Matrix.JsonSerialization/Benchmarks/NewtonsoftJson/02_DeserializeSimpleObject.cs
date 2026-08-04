using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public SimpleModel? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<SimpleModel>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Simple(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
