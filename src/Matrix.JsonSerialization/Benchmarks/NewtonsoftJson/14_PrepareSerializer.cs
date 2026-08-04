using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Serialization;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class PrepareSerializer
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public JsonContract NewtonsoftJson()
    {
        var settings = JsonConfiguration.CreateNewtonsoftDefault();
        return settings.ContractResolver!.ResolveContract(typeof(SimpleModel));
    }
}
