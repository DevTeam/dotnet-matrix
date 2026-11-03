using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class NestedObject
{
    private readonly IValidator<NestedInput> _validotNestedObject =
        ValidotSpecifications.Nested();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Validot)]
    public ValidotResult Validot()
    {
        var result = _validotNestedObject.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.Validot,
            !result.AnyErrors,
            result.Paths,
            $"{nameof(NestedInput.Address)}.{nameof(AddressInput.PostalCode)}");
        return result;
    }
}
