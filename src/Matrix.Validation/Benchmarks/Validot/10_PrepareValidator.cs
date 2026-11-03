using System.Diagnostics.CodeAnalysis;
using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class PrepareValidator
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Validot)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public IValidator<BasicInput> Validot() => ValidotSpecifications.Basic();
}
