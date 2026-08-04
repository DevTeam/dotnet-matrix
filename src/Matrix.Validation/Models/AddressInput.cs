namespace Matrix.Validation.Models;

public sealed class AddressInput
{
    [Required]
    public string Street { get; init; } = string.Empty;

    [Required]
    public string PostalCode { get; init; } = string.Empty;
}
