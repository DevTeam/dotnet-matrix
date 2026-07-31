namespace Matrix.Validation.Models;

public sealed partial class BasicInput
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(18, 120)]
    public int Age { get; set; }
}
