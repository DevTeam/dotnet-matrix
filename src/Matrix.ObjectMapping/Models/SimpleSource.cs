namespace Matrix.ObjectMapping.Models;

public sealed class SimpleSource
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Active { get; set; }
}
