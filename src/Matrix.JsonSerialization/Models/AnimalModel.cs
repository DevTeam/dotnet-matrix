using System.Text.Json.Serialization;

namespace Matrix.JsonSerialization.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(CatModel), "cat")]
[JsonDerivedType(typeof(DogModel), "dog")]
public abstract class AnimalModel
{
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;
}
