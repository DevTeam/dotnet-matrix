using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class PrepareSerializer
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public JsonTypeInfo SystemTextJson()
    {
        var options = JsonConfiguration.CreateSystemTextDefault();
        options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
        return options.GetTypeInfo(typeof(SimpleModel));
    }
}
