namespace Matrix.Validation.Models;

public sealed class LineItemInput
{
    [Required]
    public string Sku { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Quantity { get; set; }
}
