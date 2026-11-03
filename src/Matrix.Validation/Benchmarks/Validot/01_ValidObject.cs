using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public partial class ValidObject
{
    private readonly IValidator<BasicInput> _validotValidObject =
        ValidotSpecifications.Basic();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Validot)]
    public ValidotResult Validot()
    {
        var result = _validotValidObject.Validate(_input);
        ValidationChecks.Exact(
            LibraryCatalog.Validot,
            !result.AnyErrors,
            result.Paths);
        return result;
    }
}
