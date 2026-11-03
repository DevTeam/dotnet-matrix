using Validot;

namespace Matrix.Validation.Aot;

internal static class AotProbe
{
    public const string Library = "Validot";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Validates one valid object and one invalid object through a factory-created
    /// <c>IValidator&lt;T&gt;</c>, exactly like the benchmarks'
    /// <c>ValidObject</c>/<c>SingleFailure</c> scenarios minus the shared fixture, and checks both
    /// outcomes.
    /// </summary>
    public static int Run()
    {
        var validator = Validot.Validator.Factory.Create<ProbeInput>(
            input => input.Member(m => m.Name, name => name.NotEmpty()));

        var valid = validator.Validate(new ProbeInput { Name = "probe" });
        var invalid = validator.Validate(new ProbeInput { Name = string.Empty });

        return !valid.AnyErrors && invalid.Paths.Count == 1 ? 1 : 0;
    }

    private sealed class ProbeInput
    {
        public string Name { get; init; } = string.Empty;
    }
}
