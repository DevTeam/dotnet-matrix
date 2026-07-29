namespace Matrix.Validation.Models;

public sealed class ConditionalInput : IValidatableObject
{
    public bool IsBusiness { get; set; }

    public string? TaxId { get; set; }

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
