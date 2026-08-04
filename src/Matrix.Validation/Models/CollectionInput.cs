namespace Matrix.Validation.Models;

public sealed partial class CollectionInput
{
    [Required]
    public List<LineItemInput> Items { get; init; } = [];
}
