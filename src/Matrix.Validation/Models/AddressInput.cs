namespace Matrix.Validation.Models;

public sealed class AddressInput
{
    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;
}
