namespace Matrix.Validation.Models;

public sealed class CollectionInput
{
    [Required]
    public List<LineItemInput> Items { get; set; } = [];
}
