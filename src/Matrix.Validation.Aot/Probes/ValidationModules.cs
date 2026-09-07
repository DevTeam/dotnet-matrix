using ValidationModules;

namespace Matrix.Validation.Aot;

internal static class AotProbe
{
    public const string Library = "ValidationModules";

    public const int ExpectedEvents = 1;

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
