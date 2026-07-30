namespace Matrix.JsonSerialization.Models;

public sealed class SimpleModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; }
}
