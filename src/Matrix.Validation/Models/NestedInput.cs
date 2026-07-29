namespace Matrix.Validation.Models;

public sealed class NestedInput
{
    [Required]
    public AddressInput Address { get; set; } = new();
}
