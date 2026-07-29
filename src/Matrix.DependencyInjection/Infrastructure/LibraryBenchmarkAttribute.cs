namespace Matrix.DependencyInjection.Infrastructure;

[AttributeUsage(AttributeTargets.Method)]
public sealed class LibraryBenchmarkAttribute(
    string libraryId,
    bool baseline = false) : Attribute
{
    public string LibraryId { get; } = libraryId;

    public bool Baseline { get; } = baseline;
}