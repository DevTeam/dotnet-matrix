using Newtonsoft.Json.Serialization;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class PrepareSerializer
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    public JsonContract NewtonsoftJson()
    {
        var settings = JsonConfiguration.CreateNewtonsoftDefault();
        return ((IContractResolver)settings.ContractResolver!)
            .ResolveContract(typeof(SimpleModel));
    }
}
