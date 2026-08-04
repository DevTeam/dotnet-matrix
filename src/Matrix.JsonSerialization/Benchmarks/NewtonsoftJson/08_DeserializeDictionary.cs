using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public Dictionary<string, int>? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Dictionary(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
