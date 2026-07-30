using System.Text.Json.Serialization;

namespace Matrix.JsonSerialization;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(SimpleModel))]
internal sealed partial class MatrixJsonContext : JsonSerializerContext
{
}
