namespace Matrix.Validation.Benchmarks;

public partial class StopOnFirstFailure
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DataAnnotations)]
    public List<ValidationResult> DataAnnotations()
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(_input)
        {
            MemberName = nameof(BasicInput.Name)
        };
        var isValid = Validator.TryValidateProperty(_input.Name, context, results);
        if (isValid)
        {
            context.MemberName = nameof(BasicInput.Email);
            isValid = Validator.TryValidateProperty(_input.Email, context, results);
        }

        if (isValid)
        {
            context.MemberName = nameof(BasicInput.Age);
            isValid = Validator.TryValidateProperty(_input.Age, context, results);
        }

        ValidationChecks.Exact(
            LibraryCatalog.DataAnnotations,
            isValid,
            results.SelectMany(result => result.MemberNames),
            nameof(BasicInput.Name));
        return results;
    }
}
