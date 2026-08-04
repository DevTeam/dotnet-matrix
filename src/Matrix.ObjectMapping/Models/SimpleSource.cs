namespace Matrix.ObjectMapping.Models;

public sealed class SimpleSource
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Amount { get; init; }

    public DateTime CreatedAt { get; init; }

    public bool Active { get; init; }
}
