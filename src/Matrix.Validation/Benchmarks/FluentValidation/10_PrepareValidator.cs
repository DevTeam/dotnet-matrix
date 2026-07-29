using FluentValidation;

namespace Matrix.Validation.Benchmarks;

public partial class PrepareValidator
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FluentValidation)]
    public InlineValidator<BasicInput> FluentValidation() =>
        FluentValidatorFactory.Basic();
}
