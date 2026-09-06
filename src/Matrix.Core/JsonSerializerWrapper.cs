using System.Net.Http.Json;
using System.Text.Json;

namespace Matrix;

/// <summary>
/// Default <see cref="IJsonSerializer"/> backed by <see cref="JsonSerializer"/>.
/// Registered as a singleton in every composition; options travel with each call.
/// </summary>
public sealed class JsonSerializerWrapper : IJsonSerializer
{
    public string Serialize<TValue>(TValue value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(value, options);

    public TValue? Deserialize<TValue>(string json, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<TValue>(json, options);

    public ValueTask<TValue?> DeserializeAsync<TValue>(
        Stream utf8Json,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default) =>
        JsonSerializer.DeserializeAsync<TValue>(utf8Json, options, cancellationToken);

    public Task<TValue?> GetFromJsonAsync<TValue>(
        HttpClient client,
        string requestUri,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default) =>
        client.GetFromJsonAsync<TValue>(requestUri, options, cancellationToken);

    public Task<TValue?> ReadFromJsonAsync<TValue>(
        HttpContent content,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default) =>
        content.ReadFromJsonAsync<TValue>(options, cancellationToken);
}
