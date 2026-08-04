using System.Diagnostics.CodeAnalysis;
using ServiceStack.Text;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.DictionaryJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public Dictionary<string, int>? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<Dictionary<string, int>>(Input);
        SerializationChecks.Dictionary(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
