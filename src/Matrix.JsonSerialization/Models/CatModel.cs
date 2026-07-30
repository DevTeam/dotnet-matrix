namespace Matrix.JsonSerialization.Models;

public sealed class CatModel : AnimalModel
{
    [System.Text.Json.Serialization.JsonPropertyOrder(1)]
    public int Lives { get; set; }
}
