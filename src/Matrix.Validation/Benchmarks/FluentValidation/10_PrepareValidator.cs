using System.Diagnostics.CodeAnalysis;
using FluentValidation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class PrepareValidator
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public InlineValidator<BasicInput> FluentValidation() =>
        FluentValidatorFactory.Basic();
}
