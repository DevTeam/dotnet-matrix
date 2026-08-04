namespace Matrix.Validation.Models;

public sealed partial class BasicInput
{
    [Required]
    public string Name { get; init; } = string.Empty;

    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Range(18, 120)]
    public int Age { get; init; }
}
