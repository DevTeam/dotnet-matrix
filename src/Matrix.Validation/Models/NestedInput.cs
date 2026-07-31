namespace Matrix.Validation.Models;

public sealed partial class NestedInput
{
    [Required]
    public AddressInput Address { get; set; } = new();
}
