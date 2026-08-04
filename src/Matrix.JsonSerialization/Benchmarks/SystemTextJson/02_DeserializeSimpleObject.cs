using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public SimpleModel? SystemTextJson()
    {
        var result = JsonSerializer.Deserialize<SimpleModel>(
            Input,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Simple(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
