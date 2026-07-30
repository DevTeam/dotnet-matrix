using System.Text.Json.Serialization.Metadata;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class PrepareSerializer
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    public JsonTypeInfo SystemTextJson()
    {
        var options = JsonConfiguration.CreateSystemTextDefault();
        options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
        return options.GetTypeInfo(typeof(SimpleModel));
    }
}
