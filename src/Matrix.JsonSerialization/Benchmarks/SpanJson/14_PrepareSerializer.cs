using System.Diagnostics.CodeAnalysis;
using SpanJson;
using SpanJson.Resolvers;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class PrepareSerializer
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public IJsonFormatter<SimpleModel, char> SpanJson()
    {
        var resolver = new IncludeNullsOriginalCaseResolver<char>();
        return resolver.GetFormatter<SimpleModel>();
    }
}
