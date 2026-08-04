namespace Matrix.Validation.Models;

public sealed partial class ConditionalInput : IValidatableObject
{
    public bool IsBusiness { get; init; }

    public string? TaxId { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsBusiness && string.IsNullOrWhiteSpace(TaxId))
        {
            yield return new ValidationResult(
                "Tax ID is required for a business.",
                [nameof(TaxId)]);
        }
    }
}
