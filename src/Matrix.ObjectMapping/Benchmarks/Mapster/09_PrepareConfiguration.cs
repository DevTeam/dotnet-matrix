using System.Diagnostics.CodeAnalysis;
using Mapster;
// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareConfiguration
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public TypeAdapterConfig Mapster() => MapsterFactory.CreateConfiguration();
}
