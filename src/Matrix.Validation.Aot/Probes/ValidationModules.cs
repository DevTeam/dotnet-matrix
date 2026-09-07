using ValidationModules;

namespace Matrix.Validation.Aot;

internal static class AotProbe
{
    public const string Library = "ValidationModules";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Validates one valid object and one invalid object through a generated
    /// <c>IValidationRulesFor&lt;T&gt;</c> validator, exactly like the benchmarks'
    /// <c>ValidObject</c>/<c>SingleFailure</c> scenarios minus the shared fixture, and checks both
    /// outcomes. The source generator emits <c>ProbeInputValidator</c>/<c>ProbeInputRules_Rules</c>
    /// at namespace scope, so they need a top-level <see cref="ProbeInput"/> they can see: nesting
    /// it privately inside <see cref="AotProbe"/>, as every other probe does, fails generated code
    /// with CS0122 ("inaccessible due to its protection level").
    /// </summary>
    public static int Run()
    {
        var validator = new ProbeInputValidator();

        var valid = validator.Validate(new ProbeInput { Name = "probe" });
        var invalid = validator.Validate(new ProbeInput { Name = string.Empty });

        return valid.IsValid && invalid.Errors.Count == 1 ? 1 : 0;
    }
}

public sealed class ProbeInput
{
    public string Name { get; init; } = string.Empty;
}

public sealed class ProbeInputRules : IValidationRulesFor<ProbeInput>
{
    public static void Describe(ValidationRules<ProbeInput> rules, ProbeInput x) =>
        rules.Require(x.Name);
}
