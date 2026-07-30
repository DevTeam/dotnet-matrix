namespace Matrix.JsonSerialization.Models;

public sealed class DogModel : AnimalModel
{
    [System.Text.Json.Serialization.JsonPropertyOrder(1)]
    public bool GoodBoy { get; set; }
}
