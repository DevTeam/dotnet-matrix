namespace Matrix.Validation.Benchmarks;

public partial class Collection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DataAnnotations)]
    public List<ValidationResult>[] DataAnnotations()
    {
        var results = new List<ValidationResult>[_input.Items.Count + 1];
        results[0] = [];
        var isValid = Validator.TryValidateObject(
            _input,
            new ValidationContext(_input),
            results[0],
            true);
        for (var index = 0; index < _input.Items.Count; index++)
        {
            var item = _input.Items[index];
            var itemResults = new List<ValidationResult>();
            results[index + 1] = itemResults;
            isValid &= Validator.TryValidateObject(
                item,
                new ValidationContext(item),
                itemResults,
                true);
        }

        ValidationChecks.Exact(
            LibraryCatalog.DataAnnotations,
            isValid,
            results[0]
                .SelectMany(result => result.MemberNames)
                .Concat(results
                    .Skip(1)
                    .SelectMany((itemResults, index) =>
                        itemResults
                            .SelectMany(result => result.MemberNames)
                            .Select(path =>
                                $"{nameof(CollectionInput.Items)}[{index}].{path}"))),
            $"{nameof(CollectionInput.Items)}[1].{nameof(LineItemInput.Quantity)}");
        return results;
    }
}
