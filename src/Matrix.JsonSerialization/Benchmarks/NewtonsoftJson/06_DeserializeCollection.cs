using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.CollectionJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public SimpleModel[]? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<SimpleModel[]>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Collection(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
