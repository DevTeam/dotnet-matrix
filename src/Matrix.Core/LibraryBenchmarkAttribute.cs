namespace Matrix;

[AttributeUsage(AttributeTargets.Method)]
public sealed class LibraryBenchmarkAttribute(string libraryId) : Attribute
{
    public string LibraryId { get; } = libraryId;
}
