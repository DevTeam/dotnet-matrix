using System.Diagnostics.CodeAnalysis;
using AutoMapper;
// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareConfiguration
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public MapperConfiguration AutoMapper() => AutoMapperFactory.CreateConfiguration();
}
