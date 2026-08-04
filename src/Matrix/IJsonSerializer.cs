using System.Text.Json;

namespace Matrix;

/// <summary>
/// Thin wrapper around <see cref="JsonSerializer"/>. Each operation takes the
/// <see cref="JsonSerializerOptions"/> it should use, so callers stay in
/// control of indentation, naming, converters and any other per-call setting.
/// </summary>
public interface IJsonSerializer
{
    string Serialize<TValue>(TValue value, JsonSerializerOptions options);

    TValue? Deserialize<TValue>(string json, JsonSerializerOptions options);

    ValueTask<TValue?> DeserializeAsync<TValue>(
        Stream utf8Json,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default);

    Task<TValue?> GetFromJsonAsync<TValue>(
        HttpClient client,
        string requestUri,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default);

    Task<TValue?> ReadFromJsonAsync<TValue>(
        HttpContent content,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default);
}
