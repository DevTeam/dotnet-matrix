using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.DictionaryJson)]
    public Dictionary<string, int>? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<Dictionary<string, int>>(Input);
        SerializationChecks.Dictionary(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
