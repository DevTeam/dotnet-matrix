using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class ConditionalRule
{
    private readonly IValidator<ConditionalInput> _validotConditionalRule =
        ValidotSpecifications.Conditional();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Validot)]
    public ValidotResult Validot()
    {
        var result = _validotConditionalRule.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.Validot,
            !result.AnyErrors,
            result.Paths,
            nameof(ConditionalInput.TaxId));
        return result;
    }
}
