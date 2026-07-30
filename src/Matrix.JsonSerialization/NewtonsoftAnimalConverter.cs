using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Matrix.JsonSerialization;

internal sealed class NewtonsoftAnimalConverter : JsonConverter<AnimalModel>
{
    public override AnimalModel? ReadJson(
        JsonReader reader,
        Type objectType,
        AnimalModel? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var json = JObject.Load(reader);
        var discriminator = (string?)json["$type"];
        return discriminator switch
        {
            "cat" => new CatModel
            {
                Lives = json.Value<int>(nameof(CatModel.Lives)),
                Name = json.Value<string>(nameof(AnimalModel.Name)) ?? string.Empty
            },
            "dog" => new DogModel
            {
                GoodBoy = json.Value<bool>(nameof(DogModel.GoodBoy)),
                Name = json.Value<string>(nameof(AnimalModel.Name)) ?? string.Empty
            },
            _ => throw new JsonSerializationException(
                $"Unknown animal discriminator '{discriminator}'.")
        };
    }

    public override void WriteJson(
        JsonWriter writer,
        AnimalModel? value,
        JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("$type");
        switch (value)
        {
            case CatModel cat:
                writer.WriteValue("cat");
                writer.WritePropertyName(nameof(CatModel.Lives));
                writer.WriteValue(cat.Lives);
                break;
            case DogModel dog:
                writer.WriteValue("dog");
                writer.WritePropertyName(nameof(DogModel.GoodBoy));
                writer.WriteValue(dog.GoodBoy);
                break;
            default:
                throw new JsonSerializationException(
                    $"Unsupported animal type '{value?.GetType().FullName}'.");
        }

        writer.WritePropertyName(nameof(AnimalModel.Name));
        writer.WriteValue(value.Name);
        writer.WriteEndObject();
    }
}
